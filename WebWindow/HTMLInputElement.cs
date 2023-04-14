
namespace WebWindow;

public class HTMLInputElement : HTMLElement<HTMLInputElement>
{
    internal HTMLInputElement(string selector)
        : base(selector)
    { 
        _inputEventHolder = new("input", this);
    }    

    public string Type
    {
        get => Read<string>("type");
    }

    public string Value
    {
        get => Read<string>("value");
        set => Write<string>("value", value);
    }

    readonly EventHolder<Event> _inputEventHolder;

    public event Action<HTMLInputElement, Event> Input
    {
        add => _inputEventHolder.AddHandler(value);
        remove => _inputEventHolder.AddHandler(value);
    }
}