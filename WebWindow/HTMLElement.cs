
namespace WebWindow;


public abstract class HTMLElement<T> : Element<T>
    where T : HTMLElement<T>
{
    internal HTMLElement(string selector)
        : base(selector)
    { 
        _clickEvent = new("click", (T)this);
    }    

    readonly EventHolder<MouseEvent> _clickEvent;

    public event Action<T, MouseEvent> Click
    {
        add => _clickEvent.AddHandler(value);
        remove=> _clickEvent.RemoveHandler(value);
    }

    public string InnerText
    {
        get => Read<string>("innerText");
        set => Write<string>("innerText", value);
    }
}