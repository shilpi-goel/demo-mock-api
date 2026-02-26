namespace DfE.NCS.Course.Mock.Function.Models
{
    public class CourseResponse
    {
        public int TotalCourseCount { get; set; }
        public int PageNumber { get; set; }
        public int MaxPageSize { get; set; }
        public List<Course> Courses { get; set; }
    }
}
