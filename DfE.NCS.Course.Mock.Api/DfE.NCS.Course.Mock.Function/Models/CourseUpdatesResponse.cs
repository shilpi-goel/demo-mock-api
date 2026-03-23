namespace DfE.NCS.Course.Mock.Function.Models
{
    public class CourseUpdatesResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<CourseUpdate> Courses { get; set; }

        public CourseUpdatesResponse(List<CourseUpdate> courses, int totalCount, int pageNumber, int pageSize)
        {
            Courses = courses;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
