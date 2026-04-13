namespace DfE.NCS.Course.Mock.Function.Models
{
    public class TLevelUpdatesResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<TLevelUpdate> TLevels { get; set; }

        public TLevelUpdatesResponse(List<TLevelUpdate> courses, int totalCount, int pageNumber, int pageSize)
        {
            TLevels = courses;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
