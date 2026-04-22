using System;
using System.Collections.Generic;
using System.Text;

namespace DfE.NCS.Course.Mock.Function.Database;

/// <summary>
/// Cache keys constants class represents the constant values for cache keys used in the application to store and retrieve data from the cache.
/// </summary>
public class CacheKeyConstants
{
    /// <summary>
    /// Db access token cache key constant represents the key used to store and retrieve the database access token from the cache. This token is typically used for authentication and authorization purposes when accessing the database, ensuring secure and efficient access to database resources. By using a constant value for the cache key, it helps maintain consistency and reduces the risk of errors caused by hard-coded strings throughout the application.
    /// </summary>
    public const string DbAccessToken = "db:access_token";
}
