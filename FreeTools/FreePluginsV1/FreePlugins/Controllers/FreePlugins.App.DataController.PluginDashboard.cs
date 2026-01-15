using FreePlugins.Abstractions;
using FreePlugins.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plugins;

namespace FreePlugins.Server.Controllers;

#region FreePlugins - Plugin Dashboard Controller
// ============================================================================
// PLUGIN DASHBOARD API ENDPOINT
// ============================================================================
// Part of: FreePlugins.App (custom extension)
//
// PURPOSE:
//   Provides an API endpoint for the Plugin Dashboard feature.
//   Returns all registered plugins (file-based + compiled) without source code.
//
// SECURITY:
//   - Admin-only access via CurrentUser.Admin check
//   - No source code returned (security)
//   - Uses GetPluginsWithoutCode() for file-based plugins
//
// ENDPOINT:
//   GET /api/Data/GetPluginDashboard
// ============================================================================

public partial class DataController
{
    /// <summary>
    /// Gets all registered plugins for the Plugin Dashboard.
    /// Admin access required. Does not expose source code.
    /// </summary>
    [HttpGet]
    [Authorize]
    [Route("~/api/Data/GetPluginDashboard")]
    public ActionResult<DataObjects.PluginDashboardResponse> GetPluginDashboard()
    {
        var output = new DataObjects.PluginDashboardResponse();

        // Security check: Admin only
        if (CurrentUser == null || !CurrentUser.Admin)
        {
            output.Success = false;
            output.Message = "Access denied. Administrator privileges required.";
            return Ok(output);
        }

        try
        {
            var pluginList = new List<DataObjects.PluginDashboardInfo>();
            var countsByType = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // Get file-based plugins (without code for security)
            var fileBasedPlugins = da.GetPluginsWithoutCode();
            foreach (var plugin in fileBasedPlugins)
            {
                var info = new DataObjects.PluginDashboardInfo
                {
                    Id = plugin.Id,
                    Name = plugin.Name ?? string.Empty,
                    Author = plugin.Author ?? string.Empty,
                    Version = plugin.Version ?? string.Empty,
                    Type = plugin.Type ?? "General",
                    Description = GetPluginDescription(plugin),
                    Enabled = plugin.Enabled,
                    SortOrder = plugin.SortOrder,
                    IsCompiled = false,
                    ContainsSensitiveData = plugin.ContainsSensitiveData,
                    PromptCount = plugin.Prompts?.Count ?? 0,
                    LimitToTenants = plugin.LimitToTenants ?? string.Empty,
                    Source = $"File: {plugin.ClassName}.cs"
                };

                pluginList.Add(info);

                // Count by type
                string typeKey = info.Type;
                if (!countsByType.TryAdd(typeKey, 1))
                {
                    countsByType[typeKey]++;
                }
            }

            // Get compiled plugins from the registry
            var compiledPlugins = CompiledPluginRegistry.AllPlugins;
            foreach (var entry in compiledPlugins)
            {
                var metadata = entry.Metadata;
                var info = new DataObjects.PluginDashboardInfo
                {
                    Id = metadata.Id,
                    Name = metadata.Name ?? string.Empty,
                    Author = metadata.Author ?? string.Empty,
                    Version = metadata.Version ?? string.Empty,
                    Type = metadata.Type ?? "General",
                    Description = metadata.Description ?? string.Empty,
                    Enabled = metadata.Enabled,
                    SortOrder = metadata.SortOrder,
                    IsCompiled = true,
                    ContainsSensitiveData = metadata.ContainsSensitiveData,
                    PromptCount = metadata.Prompts?.Count ?? 0,
                    LimitToTenants = metadata.LimitToTenants ?? string.Empty,
                    Source = $"Compiled: {entry.PluginType.Assembly.GetName().Name}"
                };

                pluginList.Add(info);

                // Count by type
                string typeKey = info.Type;
                if (!countsByType.TryAdd(typeKey, 1))
                {
                    countsByType[typeKey]++;
                }
            }

            // Sort plugins by SortOrder, then Name
            output.Plugins = pluginList
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToList();

            // Calculate summary stats
            output.TotalCount = output.Plugins.Count;
            output.EnabledCount = output.Plugins.Count(p => p.Enabled);
            output.FileBasedCount = output.Plugins.Count(p => !p.IsCompiled);
            output.CompiledCount = output.Plugins.Count(p => p.IsCompiled);
            output.CountsByType = countsByType;
            output.Success = true;
        }
        catch (Exception ex)
        {
            output.Success = false;
            output.Message = $"Error loading plugins: {ex.Message}";
        }

        return Ok(output);
    }

    /// <summary>
    /// Gets a description for a file-based plugin from its Properties dictionary.
    /// </summary>
    private static string GetPluginDescription(Plugin plugin)
    {
        if (plugin.Properties != null && plugin.Properties.TryGetValue("Description", out var desc))
        {
            return desc?.ToString() ?? string.Empty;
        }
        return string.Empty;
    }
}

#endregion
