
namespace WebWindow;

public abstract class EventTarget<T>
    where T : EventTarget<T>
{
    protected class EventHolder<TEvent>
        where TEvent : Event
    {
        public EventHolder(T element, string evt)
        {
            _event = evt;
            _element = element;
            _handlers = new();
        }

        readonly string _event;
        readonly T _element;
        readonly List<Action<T, TEvent>> _handlers;

        void CallHandlers(TEvent e)
        {
            foreach(var h in _handlers)
            {
                h(_element, e);
            }
        }

        public void AddHandler(Action<T, TEvent> handler)
        {
            _handlers.Add(handler);
            if (_handlers.Count == 1)
            {
                JSInterop.AddEventListener<TEvent>(_element.Selector, _event, CallHandlers);
            }
        }

        public void RemoveHandler(Action<T, TEvent> handler)
        {
            if (_handlers.Count == 1)
            {
                JSInterop.RemoveEventListener<TEvent>(_element.Selector, _event, CallHandlers);
            }
            _handlers.Remove(handler);
        }
    }

    protected class Property<TValue>
    {
        public Property(T element, string propertyName)
        {
            _element = element;
            _propertyName = propertyName;
        }

        readonly T _element;
        readonly string _propertyName;
        public TValue Value
        {
            get => JSInterop.Read<TValue>($"{_element.Selector}.{_propertyName}");
            set
            {
                var sValue = value is null ? "null" : value.ToString();

                // Wrap value in double quotes if of type string
                if (value is not null && typeof(TValue) == typeof(string))
                {
                    sValue = $"\"{sValue}\"";
                }
                
                JSInterop.Write($"{_element.Selector}.{_propertyName}={sValue};");
            }
        }
    }

    protected EventTarget(string selector)
    {
        Selector = selector;
    }

    protected string Selector { get; init; }

    protected void Invoke(string method, params string[] args)
    {
        JSInterop.Invoke(method, args);
    }
}