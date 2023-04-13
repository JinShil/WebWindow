
namespace WebWindow;

public class HTMLElement : Element
{
    internal HTMLElement(string selector)
        : base(selector)
    { }

    protected string GetFunctionName(int id)
    {
        return $"_{id}";
    }

    protected void AddEventListener(string evt, int id)
    {
        var name = GetFunctionName(id);
        Emit($$"""
            function {{name}}() 
            { 
                window.webkit.messageHandlers.webview.postMessage("{{id}}"); 
            }
            """);
        Invoke($"{Selector}.addEventListener", $"\"{evt}\"", name);
    }

    protected void RemoveEventListener(string evt, int id)
    {
        var name = GetFunctionName(id);
        Invoke($"{Selector}.removeEventListener", $"\"{evt}\"", name);
    }

    public event JavascriptEventHandler Click
    {
        add => AddEventListener("click", value.GetHashCode());
        remove=> RemoveEventListener("click", value.GetHashCode());
    }
}