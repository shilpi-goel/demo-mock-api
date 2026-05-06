using System.Data.Common;

namespace DfE.NCS.Course.Mock.Function.Database
{
    /// <summary>
    /// Contract for a factory that creates database connections. This interface abstracts the creation of database connections.
    /// </summary>
    public interface ISqlDBConnectionFactory
    {
        /// <summary>
        /// Create db connection.
        /// </summary>
        /// <returns>SqlConnection.</returns>
        Task<DbConnection> CreateConnectionAsync();
    }
}
