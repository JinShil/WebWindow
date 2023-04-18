namespace WebWindow.Internal;

internal class Property<TValue>
{
    public Property(string parentSelector, string propertyName)
    {
        _selector = $"{parentSelector}.{propertyName}";
    }

    readonly string _selector;

    public TValue Value
    {
        get => JSInterop.Read<TValue>(_selector);
        set
        {
            var sValue = value is null ? "null" : value.ToString();

            // Wrap value in double quotes if of type string
            if (value is not null && typeof(TValue) == typeof(string))
            {
                sValue = $"\"{sValue}\"";
            }
            
            JSInterop.Write($"{_selector}={sValue};");
        }
    }
}