
namespace WebWindow;

public class Event
{
    internal static Event Deserialize(string s)
    {
        var jdoc = JsonDocument.Parse(s);
        if (jdoc.RootElement.TryGetProperty("clientX", out JsonElement el))
        {
            return new MouseEvent(jdoc);
        }
        else if (jdoc.RootElement.TryGetProperty("view", out el))
        {
            return new UIEvent(jdoc);
        }
        else
        {
            return new Event(jdoc);
        }
    }

    internal Event(JsonDocument jdoc)
    { 
        DotNetMethod = GetPropertyValue<int>(jdoc, nameof(DotNetMethod));
    }

    protected T? GetPropertyValue<T>(JsonDocument jdoc, string propertyName)
    {
        // Except for DotNetMethod, change to javascript letter casing
        if (propertyName != nameof(DotNetMethod))
        {
            propertyName = $"{propertyName[0].ToString().ToLower()}{propertyName[1..]}";
        }

        if (!jdoc.RootElement.TryGetProperty(propertyName, out JsonElement el))
        {
            return default(T?);
        }

        if (typeof(T) == typeof(string))
        {
            return (T?)(object?)el.GetString();
        }
        else if (typeof(T) == typeof(double))
        {
            return (T)(object)el.GetDouble();
        }
        else if (typeof(T) == typeof(int))
        {
            return (T)(object)el.GetInt32();
        }
        else
        {
            throw new NotImplementedException($"Type {typeof(T).ToString()} was not implemented");
        }
    }

    public int DotNetMethod { get; init; }
}

public class UIEvent : Event
{
    internal UIEvent(JsonDocument jdoc)
        : base(jdoc)
    { }
}

public class MouseEvent : UIEvent
{
    internal MouseEvent(JsonDocument jdoc)
        : base(jdoc)
    { 
        ClientX = GetPropertyValue<double>(jdoc, nameof(ClientX));
        ClientX = GetPropertyValue<double>(jdoc, nameof(ClientY));
    }

    public double ClientX { get; init; }

    public double ClientY { get; init; }
}

