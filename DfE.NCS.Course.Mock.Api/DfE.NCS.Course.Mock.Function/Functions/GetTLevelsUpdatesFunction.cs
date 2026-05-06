using DfE.NCS.Course.Mock.Function.Models;
using DfE.NCS.Course.Mock.Function.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DfE.NCS.Course.Mock.Function.Functions
{
    public class GetTLevelsUpdatesFunction
    {
        private readonly ILogger<GetTLevelsUpdatesFunction> _logger;
        private readonly ITLevelStore _tLevelStore;

        public GetTLevelsUpdatesFunction(ILogger<GetTLevelsUpdatesFunction> logger, ITLevelStore tLevelStore)
        {
            _logger = logger;
            _tLevelStore = tLevelStore;
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
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await errorResponse.WriteAsJsonAsync(new { error = "Invalid or missing pageNumber parameter. Must be a positive integer." });
                return errorResponse;
            }

            if (!int.TryParse(req.Query["pageSize"], out var pageSize) || pageSize < 1)
            {
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await errorResponse.WriteAsJsonAsync(new { error = "Invalid or missing pageSize parameter. Must be a positive integer." });
                return errorResponse;
            }

            var invalid = bool.TryParse(req.Query["invalid"], out var invalidValue) && invalidValue;
            var allTLevels = TLevelDataGenerator.GenerateUpdates(_paginationSettings.DefaultTLevelsTotalCount, _paginationSettings.UpdatesTotalCount, invalid);

            var filteredUpdates = allTLevels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new TLevelUpdatesResponse(filteredUpdates, allTLevels.Count, pageNumber, pageSize));
            return response;
        }
    }
}
