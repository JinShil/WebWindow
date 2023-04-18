namespace WebWindow.Dom;

public class CSSStyleDeclaration
{
    internal CSSStyleDeclaration(string selector)
    {
        Selector = $"{selector}.style";
        _cssText = new(Selector, "cssText");
        _background = new(Selector, "background");
    }

    readonly string Selector;

    readonly Property<string> _cssText;
    public string CssText
    {
        get => _cssText.Value;
        set => _cssText.Value = value;
    }

    readonly Property<string> _background;
    public string Background
    {
        get => _background.Value;
        set => _background.Value = value;
    }
}