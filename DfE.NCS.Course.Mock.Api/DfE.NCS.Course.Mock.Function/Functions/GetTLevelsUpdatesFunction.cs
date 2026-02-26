using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TLevelModel = DfE.NCS.Course.Mock.Function.Models.TLevel;

namespace DfE.NCS.Course.Mock.Function.Functions
{
    public class GetTLevelsUpdatesFunction
    {
        private readonly ILogger<GetTLevelsUpdatesFunction> _logger;

        public GetTLevelsUpdatesFunction(ILogger<GetTLevelsUpdatesFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetTLevelsUpdates")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "t-levels/updates")] HttpRequestData req)
        {
            _logger.LogInformation("GetTLevelsUpdates function processed a request.");

            if (!DateTime.TryParse(req.Query["cutOffDate"], out var cutOffDate))
            {
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await errorResponse.WriteAsJsonAsync(new { error = "Invalid or missing cutOffDate parameter. Use ISO 8601 format." });
                return errorResponse;
            }

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

            var filteredUpdates = allTLevels
                .Where(t => t.UpdateDate.HasValue && t.UpdateDate.Value >= cutOffDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(filteredUpdates);
            return response;
        }
    }
}
