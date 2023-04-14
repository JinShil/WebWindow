
namespace WebWindow;

public class HTMLInputElement : HTMLElement
{
    internal HTMLInputElement(string selector)
        : base(selector)
    { }    

    public string Type
    {
        get => Read<string>("type");
    }

    public string Value
    {
        get => Read<string>("value");
        set => Write<string>("value", value);
    }

    public event Action<JsonDocument> Input
    {
        add => AddEventListener("input", value);
        remove => RemoveEventListener("input", value);
    }
}