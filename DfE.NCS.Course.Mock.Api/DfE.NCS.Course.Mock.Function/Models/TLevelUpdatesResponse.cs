namespace DfE.NCS.Course.Mock.Function.Models
{
    public class TLevelUpdatesResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<TLevelUpdate> Courses { get; set; }

        public TLevelUpdatesResponse(List<TLevelUpdate> courses, int totalCount, int pageNumber, int pageSize)
        {
            Courses = courses;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
