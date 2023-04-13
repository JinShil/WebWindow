using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WebWindow;

static class Program
{
    delegate void void_nint_nint(nint arg0, nint arg1);
    delegate void void_nint_nint_nint(nint arg0, nint arg1, nint arg2);
    delegate void void_nint_int_nint(nint arg0, int arg1, nint arg2);

    static bool running = true;

    static int Main()
    {
        Console.WriteLine(Environment.CurrentDirectory);
        var app = gtk_application_new("BlazorWebKit.Test", ApplicationFlags.NONE);

        g_signal_connect(app, "activate", Marshal.GetFunctionPointerForDelegate<void_nint_nint>(OnActivate), nint.Zero);

        // Application.Run();
        var status = g_application_run(app, 0, nint.Zero);
        var context = g_main_context_default();

        while(running)
        {
            g_main_context_iteration(context, true);
        }

        g_object_unref(app);

        return status;
    }

    static void OnActivate(nint instance, nint data)
    {
        // Create the parent window
        var window = gtk_window_new(GtkWindowType.GTK_WINDOW_TOPLEVEL);
        gtk_window_set_default_size(window, 1024, 768);

        g_signal_connect(window, "destroy", Marshal.GetFunctionPointerForDelegate<void_nint_nint>(CloseWindow), nint.Zero);

        var context = webkit_web_context_get_default();
        webkit_web_context_set_web_extensions_directory(context, Environment.CurrentDirectory);

        // Add the BlazorWebView
        var webView = webkit_web_view_new();
        var webViewContentManager = webkit_web_view_get_user_content_manager(webView);
        webkit_user_content_manager_register_script_message_handler(webViewContentManager, "webview");
        g_signal_connect(webViewContentManager, "script-message-received::webview", Marshal.GetFunctionPointerForDelegate<void_nint_nint_nint>(HandleWebMessage), webView);
        g_signal_connect(webView, "load-changed", Marshal.GetFunctionPointerForDelegate<void_nint_int_nint>(LoadChanged), webView);
        
        
        // Allow opening developer tools
        var settings = webkit_web_view_get_settings(webView);
        webkit_settings_set_enable_developer_extras(settings, true);

        gtk_container_add(window, webView);

        gtk_widget_show_all(window);

        webkit_web_view_load_html(webView, 
        """
        <html>
            <head>
                <title>This is the title</title>
            </head>
            <body>
                Working
                <p id="testp">paragraph</p>
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
            
            var p = Dom.Document.GetElementById("testp");
            for(int i = 0; i < 100000; i++)
            {
                p.InnerHTML = i.ToString();
            }
        }
    }

    static void HandleWebMessage(nint contentManager, nint jsResult, nint webView)
    {
        var jsValue = webkit_javascript_result_get_js_value(jsResult);

        if (jsc_value_is_string(jsValue)) 
        {
            var p = jsc_value_to_string(jsValue);
            var s = Marshal.PtrToStringAuto(p);
            if (s is not null)
            {
                
            }
        }

        webkit_javascript_result_unref(jsResult);
    }

    static void CloseWindow(nint instance, nint data)
    {
        running = false;
    }
}