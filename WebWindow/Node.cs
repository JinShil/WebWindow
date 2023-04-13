
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
        Dom.Emit($"{method}({string.Join(',', args)});");
    }

    protected void Emit(string js)
    {
        Dom.Emit(js);
    }
}