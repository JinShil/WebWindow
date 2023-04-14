
namespace WebWindow;

public abstract class EventTarget<T>
    where T : EventTarget<T>
{
    protected class EventHolder<TEvent>
        where TEvent : Event
    {
        public EventHolder(string evt, T element)
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
                _element.AddEventListener<TEvent>(_event, CallHandlers);
            }
        }

        public void RemoveHandler(Action<T, TEvent> handler)
        {
            if (_handlers.Count == 1)
            {
                _element.RemoveEventListener<TEvent>(_event, CallHandlers);
            }
            _handlers.Remove(handler);
        }
    }

    void AddEventListener<TEvent>(string evt, Action<TEvent> action, bool useCapture = false)
        where TEvent : Event
    {
        Dom.AddEventListener(Selector, evt, action, useCapture);
    }

    void RemoveEventListener<TEvent>(string evt, Action<TEvent> action, bool useCapture = false)
        where TEvent : Event
    {
        Dom.RemoveEventListener(Selector, evt, action, useCapture);
    }

    protected EventTarget(string selector)
    {
        Selector = selector;
    }

    protected string Selector { get; init; }

    protected TValue Read<TValue>(string js)
    {
        return Dom.Read<TValue>($"{Selector}.{js}");
    }

    protected void Write<TValue>(string property, TValue value)
    {
        var sValue = value is null ? "null" : value.ToString();
        if (value is not null && typeof(TValue) == typeof(string))
        {
            sValue = $"\"{sValue}\"";
        }
        Dom.Write($"{Selector}.{property}={sValue};");
    }

    protected void Invoke(string method, params string[] args)
    {
        Dom.Invoke(method, args);
    }
}