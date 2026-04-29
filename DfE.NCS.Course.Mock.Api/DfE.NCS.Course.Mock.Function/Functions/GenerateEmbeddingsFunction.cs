using System.Data;
using System.Net;
using System.Text.Json;
using Azure;
using Azure.AI.OpenAI;
using DfE.NCS.Course.Mock.Function.Configuration;
using DfE.NCS.Course.Mock.Function.Database;
using DfE.NCS.Course.Mock.Function.Models.Embedding;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Embeddings;

namespace DfE.NCS.FEAT.FA.Ingestion.Service.Functions;

/// <summary>
/// Generate embeddings function is an Azure Function that generates embeddings for the given input data using OpenAI's API. It is triggered by an HTTP request and returns the generated embeddings as a response. The function uses dependency injection to access the OpenAI client and database settings, allowing for flexible configuration and integration with other services.
/// </summary>
public class GenerateEmbeddingsFunction
{
    private readonly ILogger<GenerateEmbeddingsFunction> _logger;
    private readonly OpenAIClient _openAiClient;
    private readonly EmbeddingClient _embeddingClient;
    private readonly DatabaseSettings _databaseSettings;
    private readonly ISqlDBConnectionFactory _connectoinFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateEmbeddingsFunction"/> class with the specified logger and database settings. The constructor also initializes the OpenAI client using the API key and endpoint from environment variables.
    /// </summary>
    /// <param name="logger">The logger instance for logging information and errors.</param>
    /// <param name="databaseSettings">The database settings for accessing the database.</param>
    /// <param name="connectionFactory">The database connection factory for creating database connections.</param>
    /// <param name="config">The configuration instance for accessing application settings.</param>
    public GenerateEmbeddingsFunction(ILogger<GenerateEmbeddingsFunction> logger, IOptions<DatabaseSettings> databaseSettings, ISqlDBConnectionFactory connectionFactory, IConfiguration config)
    {
        _logger = logger;
        _databaseSettings = databaseSettings.Value;
        string endPoint = config.GetValue<string>("OpenAI:Endpoint") !;
        string apiKey = config.GetValue<string>("OpenAI:APIKey") !;
        string deploymentName = config.GetValue<string>("OpenAI:EmbeddingDeploymentName") !;
        _openAiClient = new AzureOpenAIClient(
                                new Uri(endPoint),
                                new AzureKeyCredential(apiKey));

        _embeddingClient = _openAiClient.GetEmbeddingClient(deploymentName);

        _connectoinFactory = connectionFactory;
    }

    /// <summary>
    /// Generates embeddings for the given input data using OpenAI's API. This function is triggered by an HTTP request and returns the generated embeddings as a response. The function logs the processing of the request and handles any exceptions that may occur during the embedding generation process.
    /// </summary>
    /// <param name="req">The HTTP request that triggers the function.</param>
    /// <returns>An <see cref="HttpResponseData"></see> containing the generated embeddings or an error message.</returns>
    [Function("GenerateEmbeddings")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function,  "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        try
        {
            KeywordEmbeddingRequest? request = null;

            string requestBody;
            using (var reader = new StreamReader(req.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Request body is empty.");
                return badResponse;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            request = JsonSerializer.Deserialize<KeywordEmbeddingRequest>(requestBody, options);

            if (request == null ||
                string.IsNullOrWhiteSpace(request.BatchId) ||
                request.Keywords.Count == 0)
            {
                var bad = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await bad.WriteStringAsync("batchId and at least one keyword are required.");
                return bad;
            }

            foreach (var keyword in request.Keywords)
            {
                _logger.LogInformation(
                    "Processing keyword '{Keyword}' for batch '{BatchId}'",
                    keyword,
                    request.BatchId);
                var startDatetime = DateTime.UtcNow;

                // create embedding
                var embeddingResult =
                    await _embeddingClient.GenerateEmbeddingAsync(keyword);

                var endDatetime = DateTime.UtcNow;

                // vector access
                ReadOnlyMemory<float> vectorReadOnly = embeddingResult.Value.ToFloats();
                float[] vector = vectorReadOnly.ToArray();


                // RAW vector 
                byte[] embeddingVectorBytes = FloatArrayToByteArray(vector);

                // convert vector to JSON
                string embeddingJson = JsonSerializer.Serialize(vector);

                // Store one keyword at a time
                await SaveEmbeddingAsync(
                    request.BatchId,
                    keyword,
                    embeddingJson,
                    embeddingVectorBytes,
                    startDatetime,
                    endDatetime);
            }

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync(
                $"Batch '{request.BatchId}' processed successfully.");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpcted error processing the request.");
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);
            var bad = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Unexpected error occured, pleaase see detail " + ex.Message);
            return bad;
        }
    }


    private static byte[] FloatArrayToByteArray(float[] vector)
    {
        var bytes = new byte[vector.Length * sizeof(float)];
        Buffer.BlockCopy(vector, 0, bytes, 0, bytes.Length);
        return bytes;
    }


    private async Task SaveEmbeddingAsync(
            string batchId,
            string keyword,
            string embeddingJson,
            byte[] embeddingVector,
            DateTime createdDateTime,
            DateTime endDatetime)
    {
        using var connection = await _connectoinFactory.CreateConnectionAsync();
        await connection.OpenAsync();

        var command = new SqlCommand(
        """
        INSERT INTO KeywordEmbeddings
        (BatchId, Keyword, EmbeddingJson, EmbeddingVector, StartDateTime, EndDateTime)
        VALUES
        (@BatchId, @Keyword, @EmbeddingJson, @EmbeddingVector, @StartDateTime, @EndDateTime)
        """, (SqlConnection)connection);

        command.Parameters.AddWithValue("@BatchId", batchId);
        command.Parameters.AddWithValue("@Keyword", keyword);
        command.Parameters.AddWithValue("@EmbeddingJson", embeddingJson);
        command.Parameters.AddWithValue("@EmbeddingVector", SqlDbType.VarBinary).Value = embeddingVector;
        command.Parameters.AddWithValue("@StartDateTime", createdDateTime);
        command.Parameters.AddWithValue("@EndDateTime", endDatetime);

        await command.ExecuteNonQueryAsync();
    }
}