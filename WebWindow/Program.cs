namespace WebWindow;

static class Program
{
    delegate void DestroyHandler(nint widget, nint data);
    delegate void ActivateHandler(nint app, nint data);
    delegate void LoadChangedHandler(nint arg0, WebkitLoadEvent arg1, nint data);

    static int Main()
    {
        var app = gtk_application_new("WebWindow.Test", ApplicationFlags.NONE);
        g_signal_connect(app, "activate", FunctionPointer<ActivateHandler>(Activate), nint.Zero);

        var status = g_application_run(app, 0, nint.Zero);

        g_object_unref(app);

        return status;
    }

    static void Activate(nint app, nint data)
    {
        // Create the parent window
        var window = gtk_application_window_new(app);
        gtk_window_set_default_size(window, 1024, 768);
        g_signal_connect(window, "destroy", FunctionPointer<DestroyHandler>(CloseWindow), app);

        // Add the WebView
        var webView = webkit_web_view_new();
        gtk_container_add(window, webView);
        g_signal_connect(webView, "load-changed", FunctionPointer<LoadChangedHandler>(LoadChanged), webView);

        gtk_widget_show_all(window);

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
                <input id="range1" type="range" />
                <p id="p1">paragraph</p>
            </body>
        </html>
        """, null);
    }

    static void LoadChanged(nint webView, WebkitLoadEvent loadEvent, nint data)
    {
        if (loadEvent == WebkitLoadEvent.WEBKIT_LOAD_FINISHED)
        {
            try
            {
                Dom.Initialize(webView);
                
                Console.WriteLine(Dom.Document.Body.TagName);
                
                var range1 = Dom.Document.GetElementById<HTMLInputElement>("range1");
                range1.Input += OnInput;

                

                var p1 = Dom.Document.GetElementById<HTMLParagraphElement>("p1");
                p1.Click += OnClick;

                for(int i = 0; i < 100000; i++)
                {
                    p1.InnerHTML = i.ToString();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    static void OnClick(HTMLParagraphElement el, MouseEvent e)
    {
        WriteLine(e.ClientX);
    }

    static void OnInput(HTMLInputElement el, Event e)
    {
        WriteLine(el.Value);
    }

    static void CloseWindow(nint widget, nint app)
    {
        g_application_quit(app);
    }
}