using DfE.NCS.Course.Mock.Function.Models;

namespace DfE.NCS.Course.Mock.Function.Storage
{
    public interface ITLevelStore
    {
        IReadOnlyList<TLevelUpdate> TLevels { get; }
    }
}
