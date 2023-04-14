using System.Runtime.InteropServices;

namespace WebWindow;

public abstract class Element : Node
{
    protected Element(string selector)
        : base(selector)
    { }

    string? _tagName;
    public string TagName
    {
        get
        {
            if (_tagName is null)
            {
                _tagName = Read<string>("tagName");
            }

            return _tagName;
        }
    }

    public string? Id
    {
        get => Read<string?>("id");
        set => Write<string?>("id", value);
    }

    public string InnerHTML
    {
        get => Read<string>("innerHTML");
        set => Write<string>("innerHTML", value);
    }
}