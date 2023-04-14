
namespace WebWindow;

public abstract class HTMLElement : Element
{
    internal HTMLElement(string selector)
        : base(selector)
    { }    

    public event Action<JsonDocument> Click
    {
        add => AddEventListener("click", value);
        remove=> RemoveEventListener("click", value);
    }
}