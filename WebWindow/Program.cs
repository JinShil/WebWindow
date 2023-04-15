namespace WebWindow;

static class Program
{
    static int Main()
    {
        _webWindow = new WebWindow(800, 480);
        _webWindow.Activated += Activated;
        _webWindow.Loaded += Loaded;

        return _webWindow.Run();
    }

    static WebWindow _webWindow = default!;
    static HTMLSpanElement rangeValue = default!;
    static Document document = default!;

    static void Activated(WebWindow webWindow)
    {
        _webWindow.LoadHTML(
            """
            <html>
                <head>
                    <title>This is the title</title>
                </head>
                <body>
                    <div><input id="range1" type="range" /><span id="range_value"></span></dive>
                    <p id="p1">paragraph</p>
                    <button id="fs_button">Toggle Fullscreen</button>
                    <button id="close_button">Close</button>
                </body>
            </html>
            """
        );
    }

    static void Loaded(WebWindow webWindow)
    {
        try
        {
            document = new Document();
            
            var range1 = document.GetElementById<HTMLInputElement>("range1");
            range1.Input += OnInput;

            var p1 = document.GetElementById<HTMLParagraphElement>("p1");
            p1.Click += OnClick;

            var fsButton = document.GetElementById<HTMLButtonElement>("fs_button");
            fsButton.Click += ToggleFullscreen;

            var closeButton = document.GetElementById<HTMLButtonElement>("close_button");
            closeButton.Click += CloseWindow;
            
            // for(int i = 0; i < 100000; i++)
            // {
            //     p1.InnerHTML = i.ToString();
            // }

            rangeValue = document.GetElementById<HTMLSpanElement>("range_value");
            rangeValue.InnerText = range1.Value;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void CloseWindow(HTMLButtonElement el, MouseEvent e)
    {
        _webWindow.Close();
    }

    static void ToggleFullscreen(HTMLButtonElement el, MouseEvent e)
    {
        if (_webWindow.IsFullscreen)
        {
            _webWindow.LeaveFullscreen();
        }
        else
        {
            _webWindow.EnterFullscreen();
        }
    }

    static void OnClick(HTMLParagraphElement el, MouseEvent e)
    {
        WriteLine(e.ClientX);
    }

    static void OnInput(HTMLInputElement el, Event e)
    {
        rangeValue.InnerText = el.Value;
    }
}