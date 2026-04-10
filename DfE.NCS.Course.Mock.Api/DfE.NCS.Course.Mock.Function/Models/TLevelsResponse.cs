namespace DfE.NCS.Course.Mock.Function.Models
{
    public class TLevelsResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<TLevel> TLevels { get; set; }

        public TLevelsResponse(List<TLevel> courses, int totalCount, int pageNumber, int pageSize)
        {
            TLevels = courses;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
