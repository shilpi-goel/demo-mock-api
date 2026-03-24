using DfE.NCS.Course.Mock.Function.Models;
using DfE.NCS.Course.Mock.Function.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DfE.NCS.Course.Mock.Function.Functions
{
    public class GetCoursesFunction
    {
        private readonly ILogger<GetCoursesFunction> _logger;
        private readonly ICourseStore _courseStore;

        public GetCoursesFunction(ILogger<GetCoursesFunction> logger, ICourseStore courseStore)
        {
            _logger = logger;
            _courseStore = courseStore;
        }

        [Function("GetCourses")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "courses/list")] HttpRequestData req)
        {
            _logger.LogInformation("GetCourses function processed a request.");

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

            var allCourses = _courseStore.Courses;

            var paginatedCourses = allCourses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Cast<Models.Course>()
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new CoursesResponse(paginatedCourses, allCourses.Count, pageNumber, pageSize));
            return response;
        }
    }
}
