namespace FreePlugins.Abstractions;

/// <summary>
/// Context provided to plugins during execution.
/// Provides access to services without tight coupling to specific implementations.
/// </summary>
public interface IPluginContext
{
    /// <summary>
    /// Gets the plugin metadata.
    /// </summary>
    PluginMetadata Plugin { get; }
    
    /// <summary>
    /// Gets the service provider for resolving dependencies.
    /// </summary>
    IServiceProvider Services { get; }
    
    /// <summary>
    /// Gets a service of the specified type.
    /// </summary>
    T? GetService<T>() where T : class;
    
    /// <summary>
    /// Gets a required service of the specified type.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
    T GetRequiredService<T>() where T : class;
    
    /// <summary>
    /// Log an informational message.
    /// </summary>
    void LogInfo(string message);
    
    /// <summary>
    /// Log a warning message.
    /// </summary>
    void LogWarning(string message);
    
    /// <summary>
    /// Log an error message.
    /// </summary>
    void LogError(string message, Exception? exception = null);
}

/// <summary>
/// Context for auth plugins with additional HTTP context.
/// </summary>
public interface IPluginAuthContext : IPluginContext
{
    /// <summary>
    /// The URL of the request.
    /// </summary>
    string Url { get; }
    
    /// <summary>
    /// The tenant ID for the request.
    /// </summary>
    Guid TenantId { get; }
    
    /// <summary>
    /// The HTTP context for the request.
    /// </summary>
    object HttpContext { get; }
}

/// <summary>
/// Context for user update plugins.
/// </summary>
public interface IPluginUserContext : IPluginContext
{
    /// <summary>
    /// The user being updated.
    /// </summary>
    object? User { get; }
}
