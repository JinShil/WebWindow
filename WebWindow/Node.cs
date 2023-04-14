
namespace WebWindow;

public abstract class Node
{
    protected Node(string selector)
    {
        Selector = selector;
    }

    protected string Selector { get; init; }

    protected T Read<T>(string js)
    {
        return Dom.Read<T>($"{Selector}.{js}");
    }

    protected void Write<T>(string property, T value)
    {
        var sValue = value is null ? "null" : value.ToString();
        if (value is not null && typeof(T) == typeof(string))
        {
            sValue = $"\"{sValue}\"";
        }
        Dom.Write($"{Selector}.{property}={sValue};");
    }

    protected void Invoke(string method, params string[] args)
    {
        Dom.Invoke(method, args);
    }

    protected void AddEventListener(string evt, Action action)
    {
        Dom.AddEventListener(Selector, evt, action);
    }

    protected void RemoveEventListener(string evt, Action action)
    {
        Dom.RemoveEventListener(Selector, evt, action);
    }
}