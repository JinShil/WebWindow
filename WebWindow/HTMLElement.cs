
namespace WebWindow;

public class HTMLElement : Element
{
    internal HTMLElement(string selector)
        : base(selector)
    { }    

    public event Action Click
    {
        add => AddEventListener("click", value);
        remove=> RemoveEventListener("click", value);
    }
}