using Microsoft.Extensions.Configuration;

namespace DfE.NCS.Course.Mock.Function.Configuration
{
    public class PaginationSettings : IPaginationSettings
    {
        private readonly IConfiguration _configuration;

        public PaginationSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int DefaultPageSize => _configuration.GetValue("PaginationSettings:DefaultPageSize", 10);

        public int DefaultTotalCount => _configuration.GetValue("PaginationSettings:DefaultTotalCount", 100);

        public int DefaultTLevelsTotalCount => _configuration.GetValue("PaginationSettings:DefaultTLevelsTotalCount", 100);

        public int UpdatesTotalCount => _configuration.GetValue("PaginationSettings:UpdatesTotalCount", 50 );
    }
}
