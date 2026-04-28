namespace DfE.NCS.Course.Mock.Function.Configuration;

/// <summary>
/// Database settings class represents the configuration settings required to establish a connection to a database.
/// </summary>
public class DatabaseSettings
{
    /// <summary>
    /// Gets or sets the connection string used to connect to the database.
    /// </summary>
    required public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the token context for auth token retreival.
    /// </summary>
    required public string TokenContext { get; set; }

    /// <summary>
    /// Gets or sets the token cache clock skew in seconds. This property represents the amount of time, in seconds, that is added as a buffer to the expiration time of a cached token. It helps to ensure that tokens are refreshed before they actually expire, preventing potential authentication issues due to clock skew between the client and the authentication server.
    /// </summary>
    public int TokenCacheClockSkewSeconds { get; set; }

    /// <summary>
    /// Gets or sets the command timeout in seconds for SQL operations.
    /// Defaults to 30 seconds. Set to 0 for no timeout.
    /// </summary>
    public int CommandTimeoutSeconds { get; set; } = 30;
}
