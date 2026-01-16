using FreePlugins.Abstractions;

namespace FreePlugins.UIExamplePlugin;

/// <summary>
/// Example plugin demonstrating the UIElement system for injecting custom UI.
/// </summary>
[Plugin(
    Id = "b7e4f8a2-1c3d-4e5f-9a8b-0c1d2e3f4a5b",
    Name = "UI Elements Example (Compiled)",
    Type = PluginTypes.General,
    Version = "1.0.0",
    Author = "WSU EIT",
    Description = "Demonstrates the UIElement system for plugins to inject custom UI.",
    SortOrder = 5,
    Enabled = true
)]
public class UIExamplePlugin : ICompiledGeneralPlugin
{
    public static Type PluginType => typeof(UIExamplePlugin);

    public Dictionary<string, object> Properties() => new()
    {
        { "Id", Guid.Parse("b7e4f8a2-1c3d-4e5f-9a8b-0c1d2e3f4a5b") },
        { "Author", "WSU EIT" },
        { "ContainsSensitiveData", false },
        { "Description", "Demonstrates the UIElement system for custom plugin UI." },
        { "Name", "UI Elements Example (Compiled)" },
        { "SortOrder", 5 },
        { "Type", PluginTypes.General },
        { "Version", "1.0.0" },
        { "Enabled", true },
        { "UIElements", BuildUIElements() },
    };

    public async Task<PluginResult> ExecuteAsync(IPluginContext context)
    {
        await Task.CompletedTask;
        var messages = new List<string>
        {
            "UI Example Plugin Executed!",
            $"Plugin: {context.Plugin.Name}",
            $"Version: {context.Plugin.Version}",
        };
        return PluginResult.Success(messages);
    }

    private static List<UIElement> BuildUIElements() =>
    [
        UIElement.Card("UI Elements Demo", [
            UIElement.Heading("Welcome to the UI Elements System!", 4),
            UIElement.Text("Custom UI without Blazor dependencies."),
            UIElement.Alert("Defined as POCOs!", "success"),
        ], "mb-3"),

        UIElement.Row([
            UIElement.Column([
                UIElement.Card("Alerts", [
                    UIElement.Alert("Info", "info"),
                    UIElement.Alert("Warning", "warning"),
                ]),
            ], "col-md-6 mb-3"),
            UIElement.Column([
                UIElement.Card("Badges", [
                    UIElement.Badge("Primary", "primary"),
                    UIElement.Badge("Success", "success"),
                ]),
            ], "col-md-6 mb-3"),
        ]),

        UIElement.Card("Table Example", [
            UIElement.Table(
                headers: ["Name", "Type", "Status"],
                rows: [["Plugin A", "General", "Active"], ["Plugin B", "Auth", "Active"]]
            ),
        ], "mb-3"),
    ];
}
