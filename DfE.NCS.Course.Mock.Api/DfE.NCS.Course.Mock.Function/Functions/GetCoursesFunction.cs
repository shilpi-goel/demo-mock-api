using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CourseModel = DfE.NCS.Course.Mock.Function.Models.Course;

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

            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "courses.json");

            if (!File.Exists(filePath))
            {
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                await errorResponse.WriteAsJsonAsync(new { error = "Courses data file not found" });
                return errorResponse;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using var doc = JsonDocument.Parse(json);
            var coursesElement = doc.RootElement.GetProperty("courses");
            var allCourses = JsonSerializer.Deserialize<List<CourseModel>>(coursesElement.GetRawText(), options) ?? new List<CourseModel>();

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
