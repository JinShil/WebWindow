
namespace WebWindow;

public class Event
{
    public Event()
    { }

    public string DotNetMethod { get; init; } = string.Empty;
}

public class UIEvent : Event
{
    public UIEvent()
        : base()
    { }
}

public class MouseEvent : UIEvent
{
    public MouseEvent()
        : base()
    { }

    public double ClientX { get; init; }

    public double ClientY { get; init; }
}

