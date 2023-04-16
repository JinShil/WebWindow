
namespace WebWindow;


public abstract class HTMLElement<T> : Element<T>
    where T : HTMLElement<T>
{
    internal HTMLElement(string selector)
        : base(selector)
    { 
        _clickEvent = new((T)this, "click");
        _innerText = new(Selector, "innerText");
        
        Style = new CSSStyleDeclaration(Selector);
    }    

    readonly EventHolder<MouseEvent> _clickEvent;

    public event Action<T, MouseEvent> Click
    {
        add => _clickEvent.AddHandler(value);
        remove=> _clickEvent.RemoveHandler(value);
    }

    readonly Property<string> _innerText;
    public string InnerText
    {
        get => _innerText.Value;
        set => _innerText.Value = value;
    }

    public CSSStyleDeclaration Style { get; init; }
}