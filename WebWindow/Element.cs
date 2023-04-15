using System.Runtime.InteropServices;

namespace WebWindow;

public abstract class Element<T> : Node<T>
    where T : Element<T>
{
    protected Element(string selector)
        : base(selector)
    { 
        _tagName = new((T)this, "tagName");
        _id = new((T)this, "id");
        _innerHTML = new((T)this, "innerHTML");
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