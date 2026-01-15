namespace FreePlugins;

#region FreePlugins - Plugin Dashboard DTOs
// ============================================================================
// PLUGIN DASHBOARD DTOs
// ============================================================================
// Part of: FreePlugins.App (custom extension)
//
// PURPOSE:
//   Provides DTOs for the Plugin Dashboard feature which displays all
//   registered plugins (both file-based and compiled) in an admin UI.
//
// DTOs:
//   PluginDashboardInfo     ? Individual plugin summary for display
//   PluginDashboardResponse ? Response containing all plugins + summary stats
//
// SECURITY:
//   - No source code exposed (Code property excluded)
//   - Admin-only access enforced at controller level
// ============================================================================

public partial class DataObjects
{
    /// <summary>
    /// Summary information for a single plugin, used in the Plugin Dashboard.
    /// Does NOT include source code for security reasons.
    /// </summary>
    public class PluginDashboardInfo
    {
        /// <summary>
        /// The unique identifier for the plugin.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The display name of the plugin.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The plugin author.
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// The plugin version.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// The plugin type (General, Auth, BackgroundProcess, UserUpdate).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Brief description of what the plugin does.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Whether the plugin is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The sort order for plugin execution.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Whether this is a compiled plugin (from NuGet) vs file-based.
        /// </summary>
        public bool IsCompiled { get; set; }

        /// <summary>
        /// Whether this plugin contains sensitive data.
        /// </summary>
        public bool ContainsSensitiveData { get; set; }

        /// <summary>
        /// Number of prompts defined by this plugin.
        /// </summary>
        public int PromptCount { get; set; }

        /// <summary>
        /// Comma-separated list of tenant codes this plugin is limited to (if any).
        /// Empty string means available to all tenants.
        /// </summary>
        public string LimitToTenants { get; set; } = string.Empty;

        /// <summary>
        /// The source of the plugin (e.g., "File: Example1.cs" or "NuGet: FreePlugins.ExamplePlugin").
        /// </summary>
        public string Source { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response object for the Plugin Dashboard containing all plugins and summary statistics.
    /// </summary>
    public class PluginDashboardResponse
    {
        /// <summary>
        /// Whether the request was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error message if the request failed.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// List of all registered plugins.
        /// </summary>
        public List<PluginDashboardInfo> Plugins { get; set; } = [];

        /// <summary>
        /// Total number of plugins.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of enabled plugins.
        /// </summary>
        public int EnabledCount { get; set; }

        /// <summary>
        /// Number of file-based plugins.
        /// </summary>
        public int FileBasedCount { get; set; }

        /// <summary>
        /// Number of compiled (NuGet) plugins.
        /// </summary>
        public int CompiledCount { get; set; }

        /// <summary>
        /// Summary counts by plugin type.
        /// </summary>
        public Dictionary<string, int> CountsByType { get; set; } = [];
    }
}

#endregion
