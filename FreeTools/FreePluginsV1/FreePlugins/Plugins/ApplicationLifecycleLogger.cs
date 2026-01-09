using FreePlugins;
using Plugins;

namespace LifecyclePlugin
{
    /// <summary>
    /// A simple plugin that logs application lifecycle events.
    /// Demonstrates the IPluginBackgroundProcess interface.
    /// 
    /// Usage:
    ///   1. Ensure BackgroundService:Enabled = true in appsettings.json
    ///   2. Plugin runs automatically on app startup
    ///   3. Check PluginLogs.log in the application folder
    ///   4. To disable: set Enabled = false in Properties() below
    /// 
    /// Portability:
    ///   To use in another FreeCRM suite, change "using FreePlugins;" to match
    ///   the target namespace (e.g., "using FreeGLBA;" or "using CRM;")
    /// </summary>
    public class ApplicationLifecycleLogger : IPluginBackgroundProcess
    {
        // Static flag ensures we only log startup once
        private static bool _startLogged = false;

        // Log file path - same directory as appsettings.json
        private static readonly string LogFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "PluginLogs.log"
        );

        /// <summary>
        /// Plugin metadata. The framework reads this to identify and configure the plugin.
        /// </summary>
        public Dictionary<string, object> Properties() => new Dictionary<string, object>
        {
            // Unique identifier - DO NOT CHANGE after deployment
            { "Id", new Guid("b7e3f1a2-5c8d-4e9f-a1b2-c3d4e5f6a7b8") },
            
            // Plugin metadata
            { "Author", "WSU EIT" },
            { "ContainsSensitiveData", false },
            { "Description", "Logs application startup events to PluginLogs.log" },
            { "Name", "Application Lifecycle Logger" },
            
            // Execution order - negative runs early
            { "SortOrder", -1000 },
            
            // Must be "BackgroundProcess" for IPluginBackgroundProcess
            { "Type", "BackgroundProcess" },
            
            // Semantic version
            { "Version", "1.0.0" },
            
            // =============================================================
            // ON/OFF TOGGLE - Set to false to disable logging
            // =============================================================
            { "Enabled", true },
        };

        /// <summary>
        /// Called by the BackgroundProcessor on each iteration.
        /// </summary>
        /// <param name="da">DataAccess instance for database operations (unused here)</param>
        /// <param name="plugin">Plugin instance with Properties</param>
        /// <param name="iteration">Iteration counter (1 on first run)</param>
        public async Task<(bool Result, List<string>? Messages, IEnumerable<object>? Objects)> Execute(
            DataAccess da,
            Plugin plugin,
            long iteration)
        {
            await Task.CompletedTask; // Satisfy async requirement

            // Only process on first iteration
            if (_startLogged)
            {
                return (Result: true, Messages: null, Objects: null);
            }

            _startLogged = true;

            // Check if logging is enabled
            bool enabled = true;
            if (plugin.Properties != null && plugin.Properties.ContainsKey("Enabled"))
            {
                try
                {
                    enabled = (bool)plugin.Properties["Enabled"];
                }
                catch
                {
                    // If we can't read Enabled, default to true
                    enabled = true;
                }
            }

            if (!enabled)
            {
                return (Result: true, Messages: new List<string> { "Lifecycle logging disabled" }, Objects: null);
            }

            // Log the startup event
            LogEvent("APP_START", "Application started successfully");

            return (Result: true, Messages: new List<string> { "Logged application startup" }, Objects: null);
        }

        /// <summary>
        /// Writes a single log entry to the log file.
        /// NOTE: Not thread-safe for multiple app instances writing to the same file.
        /// For multi-instance deployments, use unique log file names per instance.
        /// </summary>
        private void LogEvent(string eventType, string message)
        {
            try
            {
                // Create header if file doesn't exist
                if (!File.Exists(LogFilePath))
                {
                    File.WriteAllText(LogFilePath, "Timestamp,Event,MachineName,ProcessId,Message\n");
                }

                // Build CSV entry
                string entry = string.Format("{0},{1},{2},{3},{4}\n",
                    DateTime.UtcNow.ToString("o"),           // ISO 8601 timestamp
                    eventType,                                // Event type
                    Environment.MachineName,                  // Server name
                    Environment.ProcessId,                    // Process ID
                    EscapeCsvField(message)                   // Message (escaped)
                );

                // Append to file
                File.AppendAllText(LogFilePath, entry);
            }
            catch
            {
                // Swallow all exceptions - logging should never crash the app
                // The BackgroundProcessor will log any Messages we return
            }
        }

        /// <summary>
        /// Escapes a string for CSV format (handles commas and quotes).
        /// </summary>
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return string.Empty;
            }

            // If field contains comma, quote, or newline, wrap in quotes and escape internal quotes
            if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }

            return field;
        }
    }
}
