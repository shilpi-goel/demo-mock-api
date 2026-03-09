using DfE.NCS.Course.Mock.Function.DataGenerators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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

            var allTLevels = TLevelDataGenerator.Generate(100);

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
