
namespace WebWindow;

public abstract class Node : EventTarget
{
    protected Node(string selector)
        : base(selector)
    { }
}