namespace WebWindow;

public abstract class Element<T> : Node<T>
    where T : Element<T>
{
    protected Element(string selector)
        : base(selector)
    { 
        _tagName = new(Selector, "tagName");
        _id = new(Selector, "id");
        _innerHTML = new(Selector, "innerHTML");
    }

    readonly Property<string> _tagName;
    public string TagName
    {
        get => _tagName.Value;
    }

    readonly Property<string> _id;
    public string Id
    {
        get => _id.Value;
        set => _id.Value = value;
    }

    readonly Property<string> _innerHTML;
    public string InnerHTML
    {
        get => _innerHTML.Value;
        set => _innerHTML.Value = value;
    }
}