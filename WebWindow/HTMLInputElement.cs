
namespace WebWindow;

public class HTMLInputElement : HTMLElement<HTMLInputElement>
{
    internal HTMLInputElement(string selector)
        : base(selector)
    { 
        _inputEventHolder = new(this, "input");
        _type = new(Selector, "type");
        _value = new(Selector, "value");
    }    

    readonly Property<string> _type;
    public string Type
    {
        get => _type.Value;
    }

    readonly Property<string> _value;
    public string Value
    {
        get => _value.Value;
        set => _value.Value = value;
    }

    readonly EventHolder<Event> _inputEventHolder;

    public event Action<HTMLInputElement, Event> Input
    {
        add => _inputEventHolder.AddHandler(value);
        remove => _inputEventHolder.AddHandler(value);
    }
}