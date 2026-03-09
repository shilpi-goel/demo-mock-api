using DfE.NCS.Course.Mock.Function.DataGenerators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DfE.NCS.Course.Mock.Function.Functions
{
    public class GetCoursesFunction
    {
        private readonly ILogger<GetCoursesFunction> _logger;

        public GetCoursesFunction(ILogger<GetCoursesFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetCourses")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "courses/list")] HttpRequestData req)
        {
            _logger.LogInformation("GetCourses function processed a request.");

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
                totalCount = 50;
            }

            var allCourses = CourseDataGenerator.Generate(totalCount);

            var paginatedCourses = allCourses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(paginatedCourses);
            return response;
        }
    }
}
