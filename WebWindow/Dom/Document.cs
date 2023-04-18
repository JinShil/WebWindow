using WebWindow.Dom;

public class Document : Node<Document>
{
    public Document()
        : base("document")
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
        var tagName = JSInterop.Read<string>($"{selector}.tagName");

        if (tagName == "BODY")
        {
            return (T)(object)new HTMLBodyElement(selector);
        }
        else if (tagName == "BUTTON")
        {
            return (T)(object)new HTMLButtonElement(selector);
        }
        else if (tagName == "DIV")
        {
            return (T)(object)new HTMLDivElement(selector);
        }
        if (tagName == "INPUT")
        {
            return (T)(object)new HTMLInputElement(selector);
        }
        else if (tagName == "P")
        {
            return (T)(object)new HTMLParagraphElement(selector);
        }
        else if (tagName == "SPAN")
        {
            return (T)(object)new HTMLSpanElement(selector);
        }
        
        throw new NotImplementedException();
    }
}