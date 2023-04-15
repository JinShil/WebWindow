using System.Runtime.InteropServices;

namespace WebWindow;

static class Program
{
    delegate void void_nint_nint(nint arg0, nint arg1);
    delegate void void_nint_nint_nint(nint arg0, nint arg1, nint arg2);
    delegate void void_nint_int_nint(nint arg0, int arg1, nint arg2);

    static int Main()
    {
        var app = gtk_application_new("WebWindow.Test", ApplicationFlags.NONE);
        g_signal_connect(app, "activate", Marshal.GetFunctionPointerForDelegate<void_nint_nint>(OnActivate), nint.Zero);

        var status = g_application_run(app, 0, nint.Zero);

        g_object_unref(app);

        return status;
    }

    static void OnActivate(nint app, nint data)
    {
        // Create the parent window
        var window = gtk_application_window_new(app);
        gtk_window_set_default_size(window, 1024, 768);
        g_signal_connect(window, "destroy", Marshal.GetFunctionPointerForDelegate<void_nint_nint>(CloseWindow), app);

        // Add the WebView
        var webView = webkit_web_view_new();
        gtk_container_add(window, webView);
        g_signal_connect(webView, "load-changed", Marshal.GetFunctionPointerForDelegate<void_nint_int_nint>(LoadChanged), webView);

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

    static void LoadChanged(nint webView, int loadEvent, nint data)
    {
        if (loadEvent == 3)
        {
            Dom.Initialize(webView);
            
            Console.WriteLine(Dom.Document.Body.TagName);
            // Dom.Document.Body.InnerHTML = (Process.GetCurrentProcess().StartTime - DateTime.Now).ToString();
            
            var range1 = Dom.Document.GetElementById<HTMLInputElement>("range1");
            range1.Input += OnInput;

            // for(int i = 0; i < 100000; i++)
            // {
            //     p.InnerHTML = i.ToString();
            // }

            var p1 = Dom.Document.GetElementById<HTMLParagraphElement>("p1");
            p1.Click += OnClick;
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