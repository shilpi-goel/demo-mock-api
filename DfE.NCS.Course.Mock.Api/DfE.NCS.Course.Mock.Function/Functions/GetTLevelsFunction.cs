using DfE.NCS.Course.Mock.Function.Configuration;
using DfE.NCS.Course.Mock.Function.DataGenerators;
using DfE.NCS.Course.Mock.Function.Models;
using DfE.NCS.Course.Mock.Function.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DfE.NCS.Course.Mock.Function.Functions
{
    public class GetTLevelsFunction
    {
        private readonly ILogger<GetTLevelsFunction> _logger;
        private readonly ITLevelStore _tLevelStore;
        private readonly IPaginationSettings _paginationSettings;

        public GetTLevelsFunction(ILogger<GetTLevelsFunction> logger, ITLevelStore tLevelStore, IPaginationSettings paginationSettings)
        {
            _logger = logger;
            _tLevelStore = tLevelStore;
            _paginationSettings = paginationSettings;

        }

        [Function("GetTLevels")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "t-levels/list")] HttpRequestData req)
        {
            _logger.LogInformation("GetTLevels function processed a request.");

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

            var paginatedTLevels = allTLevels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Cast<Models.TLevel>()
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new TLevelsResponse(paginatedTLevels, allTLevels.Count, pageNumber, pageSize));
            return response;
        }
    }
}
