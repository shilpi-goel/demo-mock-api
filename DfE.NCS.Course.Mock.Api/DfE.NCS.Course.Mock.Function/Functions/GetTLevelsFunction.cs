using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TLevelModel = DfE.NCS.Course.Mock.Function.Models.TLevel;

namespace DfE.NCS.Course.Mock.Function.Functions
{
    public class GetTLevelsFunction
    {
        private readonly ILogger<GetTLevelsFunction> _logger;

        public GetTLevelsFunction(ILogger<GetTLevelsFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetTLevels")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "t-levels/list")] HttpRequestData req)
        {
            _logger.LogInformation("GetTLevels function processed a request.");

            if (!int.TryParse(req.Query["pageNumber"], out var pageNumber) || pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (!int.TryParse(req.Query["pageSize"], out var pageSize) || pageSize < 1)
            {
                pageSize = 10;
            }

            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "t-level.json");

            if (!File.Exists(filePath))
            {
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                await errorResponse.WriteAsJsonAsync(new { error = "T-Levels data file not found" });
                return errorResponse;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using var doc = JsonDocument.Parse(json);
            var coursesElement = doc.RootElement.GetProperty("courses");
            var allTLevels = JsonSerializer.Deserialize<List<TLevelModel>>(coursesElement.GetRawText(), options) ?? new List<TLevelModel>();

            var paginatedTLevels = allTLevels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(paginatedTLevels);
            return response;
        }
    }
}
