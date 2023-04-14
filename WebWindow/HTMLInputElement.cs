
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

    class EventHolder<T>
        where T : EventTarget
    {
        public EventHolder(T element, Action<T, JsonDocument> handler)
        {
            _element = element;
            _handler = handler;
        }

        readonly T _element;
        readonly Action<T, JsonDocument> _handler;

        public void CallHandler(JsonDocument e)
        {
            _handler(_element, e);
        }
    }

    EventHolder<HTMLInputElement>? _inputEventHolder;

    public event Action<HTMLInputElement, JsonDocument> Input
    {
        add
        {
            if (_inputEventHolder is null)
            {
                _inputEventHolder = new EventHolder<HTMLInputElement>(this, value);
            }

            AddEventListener("input", _inputEventHolder.CallHandler);
        }
        remove
        {
            if (_inputEventHolder is null)
            {
                return;
            }

            RemoveEventListener("input", _inputEventHolder.CallHandler);
        }
    }
}