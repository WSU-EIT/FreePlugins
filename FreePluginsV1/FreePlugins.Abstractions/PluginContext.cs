using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FreePlugins.Abstractions;

/// <summary>
/// Default implementation of IPluginContext for compiled plugins.
/// </summary>
public class PluginContext : IPluginContext
{
    private readonly IServiceProvider _services;
    private readonly ILogger? _logger;

    public PluginContext(PluginMetadata plugin, IServiceProvider services, ILogger? logger = null)
    {
        Plugin = plugin;
        _services = services;
        _logger = logger;
    }

    public PluginMetadata Plugin { get; }

    public IServiceProvider Services => _services;

    public T? GetService<T>() where T : class
    {
        return _services.GetService<T>();
    }

    public T GetRequiredService<T>() where T : class
    {
        return _services.GetRequiredService<T>();
    }

    public void LogInfo(string message)
    {
        _logger?.LogInformation("[{PluginName}] {Message}", Plugin.Name, message);
        Console.WriteLine($"[{Plugin.Name}] INFO: {message}");
    }

    public void LogWarning(string message)
    {
        _logger?.LogWarning("[{PluginName}] {Message}", Plugin.Name, message);
        Console.WriteLine($"[{Plugin.Name}] WARN: {message}");
    }

    public void LogError(string message, Exception? exception = null)
    {
        if (exception != null)
        {
            _logger?.LogError(exception, "[{PluginName}] {Message}", Plugin.Name, message);
            Console.WriteLine($"[{Plugin.Name}] ERROR: {message} - {exception.Message}");
        }
        else
        {
            _logger?.LogError("[{PluginName}] {Message}", Plugin.Name, message);
            Console.WriteLine($"[{Plugin.Name}] ERROR: {message}");
        }
    }
}

/// <summary>
/// Default implementation of IPluginAuthContext for compiled auth plugins.
/// </summary>
public class PluginAuthContext : PluginContext, IPluginAuthContext
{
    public PluginAuthContext(
        PluginMetadata plugin, 
        IServiceProvider services, 
        string url, 
        Guid tenantId, 
        object httpContext,
        ILogger? logger = null) 
        : base(plugin, services, logger)
    {
        Url = url;
        TenantId = tenantId;
        HttpContext = httpContext;
    }

    public string Url { get; }
    public Guid TenantId { get; }
    public object HttpContext { get; }
}

/// <summary>
/// Default implementation of IPluginUserContext for compiled user update plugins.
/// </summary>
public class PluginUserContext : PluginContext, IPluginUserContext
{
    public PluginUserContext(
        PluginMetadata plugin, 
        IServiceProvider services, 
        object? user,
        ILogger? logger = null) 
        : base(plugin, services, logger)
    {
        User = user;
    }

    public object? User { get; }
}
