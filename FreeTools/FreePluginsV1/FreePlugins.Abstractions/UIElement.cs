namespace FreePlugins.Abstractions;

/// <summary>
/// Defines a UI element that plugins can use to inject custom UI into the host application.
/// This is a POCO class with no Blazor dependencies.
/// </summary>
public class UIElement
{
    public UIElementType Type { get; set; } = UIElementType.Text;
    public string Text { get; set; } = string.Empty;
    public string CssClass { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public int Level { get; set; } = 3;
    public string Icon { get; set; } = string.Empty;
    public string Html { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;
    public Dictionary<string, string> Attributes { get; set; } = [];
    public List<UIElement> Children { get; set; } = [];
    public List<string> Headers { get; set; } = [];
    public List<List<string>> Rows { get; set; } = [];
    public string Variant { get; set; } = "primary";

    // Factory methods
    public static UIElement Text(string text, string cssClass = "") 
        => new() { Type = UIElementType.Text, Text = text, CssClass = cssClass };
    
    public static UIElement Heading(string text, int level = 3, string cssClass = "") 
        => new() { Type = UIElementType.Heading, Text = text, Level = level, CssClass = cssClass };
    
    public static UIElement Button(string text, string action, string cssClass = "btn-primary", string icon = "") 
        => new() { Type = UIElementType.Button, Text = text, Action = action, CssClass = cssClass, Icon = icon };
    
    public static UIElement Card(string title, IEnumerable<UIElement>? children = null, string cssClass = "") 
        => new() { Type = UIElementType.Card, Text = title, Children = children?.ToList() ?? [], CssClass = cssClass };
    
    public static UIElement Alert(string text, string variant = "info", string cssClass = "") 
        => new() { Type = UIElementType.Alert, Text = text, Variant = variant, CssClass = cssClass };
    
    public static UIElement Badge(string text, string variant = "primary", string cssClass = "") 
        => new() { Type = UIElementType.Badge, Text = text, Variant = variant, CssClass = cssClass };
    
    public static UIElement Divider(string cssClass = "") 
        => new() { Type = UIElementType.Divider, CssClass = cssClass };
    
    public static UIElement RawHtml(string html, string cssClass = "") 
        => new() { Type = UIElementType.Html, Html = html, CssClass = cssClass };
    
    public static UIElement Link(string text, string url, string cssClass = "", string icon = "") 
        => new() { Type = UIElementType.Link, Text = text, Url = url, CssClass = cssClass, Icon = icon };
    
    public static UIElement Image(string url, string alt = "", string cssClass = "") 
        => new() { Type = UIElementType.Image, Url = url, Alt = alt, CssClass = cssClass };
    
    public static UIElement List(IEnumerable<string> items, bool ordered = false, string cssClass = "") 
        => new() { Type = ordered ? UIElementType.OrderedList : UIElementType.UnorderedList, Children = items.Select(i => Text(i)).ToList(), CssClass = cssClass };
    
    public static UIElement Table(IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows, string cssClass = "") 
        => new() { Type = UIElementType.Table, Headers = headers.ToList(), Rows = rows.Select(r => r.ToList()).ToList(), CssClass = cssClass };
    
    public static UIElement Row(IEnumerable<UIElement>? children = null, string cssClass = "") 
        => new() { Type = UIElementType.Row, Children = children?.ToList() ?? [], CssClass = cssClass };
    
    public static UIElement Column(IEnumerable<UIElement>? children = null, string cssClass = "col") 
        => new() { Type = UIElementType.Column, Children = children?.ToList() ?? [], CssClass = cssClass };
    
    public static UIElement Progress(int value, int max = 100, string variant = "primary", string cssClass = "") 
        => new() { Type = UIElementType.Progress, Level = value, Attributes = new() { ["max"] = max.ToString() }, Variant = variant, CssClass = cssClass };
    
    public static UIElement IconElement(string icon, string cssClass = "") 
        => new() { Type = UIElementType.Icon, Icon = icon, CssClass = cssClass };
    
    public static UIElement Code(string code, string cssClass = "") 
        => new() { Type = UIElementType.Code, Text = code, CssClass = cssClass };
    
    public static UIElement Spacer(int size = 2) 
        => new() { Type = UIElementType.Spacer, Level = size };
}

public enum UIElementType 
{ 
    Text, Heading, Button, Card, Alert, Badge, Divider, Html, 
    Link, Image, UnorderedList, OrderedList, Table, Row, Column, 
    Progress, Icon, Code, Spacer 
}
