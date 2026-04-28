namespace DfE.NCS.Course.Mock.Function.Configuration
{
    public interface IPaginationSettings
    {
        int DefaultPageSize { get; }
        int DefaultTotalCount { get; }
        int DefaultTLevelsTotalCount { get; }
        int UpdatesTotalCount { get; }
    }
}
