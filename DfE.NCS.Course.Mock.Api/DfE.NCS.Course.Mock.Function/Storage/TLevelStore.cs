using DfE.NCS.Course.Mock.Function.Configuration;
using DfE.NCS.Course.Mock.Function.DataGenerators;
using DfE.NCS.Course.Mock.Function.Models;

namespace DfE.NCS.Course.Mock.Function.Storage
{
    public class TLevelStore : ITLevelStore
    {
        public IReadOnlyList<TLevelUpdate> TLevels { get; }

        public TLevelStore(IPaginationSettings paginationSettings)
        {
            TLevels = TLevelDataGenerator.GenerateUpdates(paginationSettings.DefaultTLevelsTotalCount);
        }
    }
}
