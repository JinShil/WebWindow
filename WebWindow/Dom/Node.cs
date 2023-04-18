namespace WebWindow.Dom;

public abstract class Node<T> : EventTarget<T>
    where T : Node<T>
{
    protected Node(string selector)
        : base(selector)
    { }
}