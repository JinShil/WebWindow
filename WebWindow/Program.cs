namespace WebWindow;

static class Program
{
    delegate void DestroyHandler(nint widget, nint data);
    delegate void ActivateHandler(nint app, nint data);
    delegate void LoadChangedHandler(nint arg0, WebkitLoadEvent arg1, nint data);

    static int Main()
    {
        var app = gtk_application_new("WebWindow.Test", GApplicationFlags.G_APPLICATION_FLAGS_NONE);
        g_signal_connect(app, "activate", FunctionPointer<ActivateHandler>(Activate), nint.Zero);

        var status = g_application_run(app, 0, nint.Zero);

        g_object_unref(app);

        return status;
    }

    static nint _window;

    static void Activate(nint app, nint data)
    {
        // Create the parent window
        _window = gtk_application_window_new(app);
        gtk_window_set_default_size(_window, 1024, 768);
        g_signal_connect(_window, "destroy", FunctionPointer<DestroyHandler>(CloseWindow), app);

        // Add the WebView
        var webView = webkit_web_view_new();
        gtk_container_add(_window, webView);
        g_signal_connect(webView, "load-changed", FunctionPointer<LoadChangedHandler>(LoadChanged), webView);

        // Allow opening developer tools
        var settings = webkit_web_view_get_settings(webView);
        webkit_settings_set_enable_developer_extras(settings, true);

        webkit_web_view_load_html(webView, 
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
        """, null);

        // Show the window on the screen
        gtk_widget_show_all(_window);
    }

    static void CloseWindow(nint widget, nint app)
    {
        g_application_quit(app);
    }

    static HTMLSpanElement rangeValue = default!;

    static void LoadChanged(nint webView, WebkitLoadEvent loadEvent, nint data)
    {
        if (loadEvent == WebkitLoadEvent.WEBKIT_LOAD_FINISHED)
        {
            try
            {
                Dom.Initialize(webView);
                
                var range1 = Dom.Document.GetElementById<HTMLInputElement>("range1");
                range1.Input += OnInput;

                var p1 = Dom.Document.GetElementById<HTMLParagraphElement>("p1");
                p1.Click += OnClick;

                var fsButton = Dom.Document.GetElementById<HTMLButtonElement>("fs_button");
                fsButton.Click += ToggleFullscreen;

                var closeButton = Dom.Document.GetElementById<HTMLButtonElement>("close_button");
                closeButton.Click += CloseWindow;
                // for(int i = 0; i < 100000; i++)
                // {
                //     p1.InnerHTML = i.ToString();
                // }

                rangeValue = Dom.Document.GetElementById<HTMLSpanElement>("range_value");
                rangeValue.InnerText = range1.Value;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    static void CloseWindow(HTMLButtonElement el, MouseEvent e)
    {
        gtk_window_close(_window);
    }

    static void ToggleFullscreen(HTMLButtonElement el, MouseEvent e)
    {
        var gdkWindow = gtk_widget_get_window(_window);
        var state = gdk_window_get_state(gdkWindow);

        if (state.HasFlag(GdkWindowState.GDK_WINDOW_STATE_FULLSCREEN))
        {
            gtk_window_unfullscreen(_window);
        }
        else
        {
            gtk_window_fullscreen(_window);
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