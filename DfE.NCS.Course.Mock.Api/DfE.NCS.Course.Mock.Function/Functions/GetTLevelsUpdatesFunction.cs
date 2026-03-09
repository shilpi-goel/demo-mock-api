using DfE.NCS.Course.Mock.Function.DataGenerators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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

            if (!int.TryParse(req.Query["totalCount"], out var totalCount) || totalCount < 1)
            {
                totalCount = 100;
            }

            var allTLevels = TLevelDataGenerator.GenerateUpdates(totalCount);

            var filteredUpdates = allTLevels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(filteredUpdates);
            return response;
        }
    }
}
