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
                _body = new HTMLBodyElement($"{Selector}.body");
            }

            return _body;
        }
    }

    public HTMLElement GetElementById(string id)
    {
        var selector = $"{Selector}.getElementById(\"{id}\")";
        var tagName = Dom.Read<string>($"{selector}.tagName");

        if (tagName == "INPUT")
        {
            return new HTMLInputElement(selector);
        }
        
        throw new NotImplementedException();
    }
}