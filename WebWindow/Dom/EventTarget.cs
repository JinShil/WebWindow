namespace WebWindow.Dom;

static file class IdFactory
{
    static int _lastId = 0;
    public static int GetId()
    {
        return ++_lastId;
    }
}

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

        readonly int _handlerId = IdFactory.GetId();
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
                JSInterop.AddEventListener<TEvent>(_element.Selector, _event, CallHandlers, _handlerId);
            }
        }

        public void RemoveHandler(Action<T, TEvent> handler)
        {
            if (_handlers.Count == 1)
            {
                JSInterop.RemoveEventListener<TEvent>(_element.Selector, _event, _handlerId);
            }
            _handlers.Remove(handler);
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