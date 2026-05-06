using DfE.NCS.Course.Mock.Function.Configuration;
using DfE.NCS.Course.Mock.Function.DataGenerators;
using DfE.NCS.Course.Mock.Function.Models;
using DfE.NCS.Course.Mock.Function.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DfE.NCS.Course.Mock.Function.Functions
{
    public class GetCoursesUpdatesFunction
    {
        private readonly ILogger<GetCoursesUpdatesFunction> _logger;
        private readonly ICourseStore _courseStore;
        private readonly IPaginationSettings _paginationSettings;

        public GetCoursesUpdatesFunction(ILogger<GetCoursesUpdatesFunction> logger, ICourseStore courseStore, IPaginationSettings paginationSettings)
        {
            _logger = logger;
            _courseStore = courseStore;
            _paginationSettings = paginationSettings;
        }

        [Function("GetCoursesUpdates")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "courses/updates")] HttpRequestData req)
        {
            _logger.LogInformation("GetCoursesUpdates function processed a request.");

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
            var allCourses = CourseDataGenerator.GenerateUpdates(_paginationSettings.DefaultTLevelsTotalCount, invalid);

            var filteredUpdates = allCourses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new CourseUpdatesResponse(filteredUpdates, allCourses.Count, pageNumber, pageSize));
            return response;
        }
    }
}
