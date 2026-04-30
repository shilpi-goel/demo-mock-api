namespace DfE.NCS.Course.Mock.Function.Models
{
    public class TLevelsResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<TLevel> Courses { get; set; }

        public TLevelsResponse(List<TLevel> courses, int totalCount, int pageNumber, int pageSize)
        {
            Courses = courses;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
