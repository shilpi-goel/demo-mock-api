using System.Data;
using System.Data.Common;
using Azure.Core;
using Azure.Identity;
using DfE.NCS.Course.Mock.Function.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DfE.NCS.Course.Mock.Function.Database;

/// <summary>
/// SqlConnectionFactory is responsible for creating and managing SQL database connections using Azure Active Directory authentication.
/// It implements the IDBConnectionFactory interface, providing a method to create and open a connection to the SQL database. 
/// </summary>
public class SqlConnectionFactory : ISqlDBConnectionFactory
{
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private readonly TokenCredential _credential;
    private readonly TokenRequestContext _tokenContext;
    private readonly DatabaseSettings _settings;
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlConnectionFactory"/> class with the specified connection string. The connection string is used to establish a connection to the SQL database.
    /// </summary>
    /// <param name="settings">Configuration object holding the db settings.</param>
    /// <param name="cache">Memory cache to store the access token.</param>
    /// <param name="credential">Azure credential for authentication. If not provided, DefaultAzureCredential will be used.</param>
    public SqlConnectionFactory(IOptions<DatabaseSettings> settings, IMemoryCache cache, TokenCredential credential)
    {
        _settings = settings.Value;
        _cache = cache;
        _credential = credential;
        _tokenContext = new TokenRequestContext([_settings.TokenContext]);
    }

    /// <summary>
    /// Create sql connection asynchronously.
    /// This method retrieves an access token using Azure Active Directory authentication and uses it to create and open a connection to the SQL database.
    /// The connection is returned as an IDbConnection instance.
    /// </summary>
    /// <returns>Sql connection.</returns>
    public async Task<DbConnection> CreateConnectionAsync()
    {
        var token = await GetAccessTokenAsync();

        var conn = new SqlConnection(_settings.ConnectionString)
        {
            AccessToken = token,
        };

        return conn;
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        if (_cache.TryGetValue(CacheKeyConstants.DbAccessToken, out string? cachedToken))
        {
            return cachedToken;
        }

        await _lock.WaitAsync();
        try
        {
            // Double-check after acquiring lock
            if (_cache.TryGetValue(CacheKeyConstants.DbAccessToken, out cachedToken))
            {
                return cachedToken;
            }

            // Get new token
            var result = await _credential.GetTokenAsync(_tokenContext, CancellationToken.None);
            TimeSpan clockSkew = TimeSpan.FromSeconds(_settings.TokenCacheClockSkewSeconds);

            // Cache expiry minus clock skew
            var cacheExpiry = result.ExpiresOn - clockSkew;

            // Store token
            _cache.Set(
                CacheKeyConstants.DbAccessToken,
                result.Token,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = cacheExpiry,
                });

            return result.Token;
        }
        finally
        {
            _lock.Release();
        }
    }
}
