using WebWindow;

public class Document : Node
{
    public Document(string selector)
        : base(selector)
    { }

    HTMLElement? _body;
    public HTMLElement Body
    {
        get
        {
            if (_body is null)
            {
                _body = new HTMLElement($"{Selector}.body");
            }

            return _body;
        }
    }

    public HTMLElement GetElementById(string id)
    {
        return new HTMLElement($"{Selector}.getElementById(\"{id}\")");
    }
}