using System.Runtime.InteropServices;

namespace WebWindow;

public abstract class Element : Node
{
    protected Element(string selector)
        : base(selector)
    { }

    public string TagName
    {
        get
        {
            return Read<string>("tagName");
        }
    }

    public string InnerHTML
    {
        get => Read<string>("innerHTML");
        set => Write<string>("innerHTML", value);
    }
}