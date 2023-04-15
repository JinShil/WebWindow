using WebWindow;

public class Document : Node<Document>
{
    public Document(string selector)
        : base(selector)
    { }

    HTMLBodyElement? _body;
    public HTMLBodyElement Body
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

    public T GetElementById<T>(string id)
        where T : HTMLElement<T>
    {
        var selector = $"{Selector}.getElementById(\"{id}\")";
        var tagName = Dom.Read<string>($"{selector}.tagName");

        if (tagName == "BUTTON")
        {
            return (T)(object)new HTMLButtonElement(selector);
        }
        if (tagName == "INPUT")
        {
            return (T)(object)new HTMLInputElement(selector);
        }
        else if (tagName == "P")
        {
            return (T)(object)new HTMLParagraphElement(selector);
        }
        
        throw new NotImplementedException();
    }
}