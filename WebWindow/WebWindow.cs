namespace WebWindow;

public class WebWindow
{
    delegate void DestroyHandler(nint widget, nint data);
    delegate void ActivateHandler(nint app, nint data);
    delegate void LoadChangedHandler(nint arg0, WebkitLoadEvent arg1, nint data);

    public WebWindow(int width = 1024, int height = 768)
    {
        _defaultWidth = width;
        _defaultHeight = height;

        _app = gtk_application_new("WebWindow.Test", GApplicationFlags.G_APPLICATION_FLAGS_NONE);
        g_signal_connect(_app, "activate", FunctionPointer<ActivateHandler>(Activate), nint.Zero);
    }

    readonly nint _app;
    readonly int _defaultWidth;
    readonly int _defaultHeight;
    nint _window;
    nint _webView;
    nint _settings;

    void Activate(nint app, nint data)
    {
        // Create the parent window
        _window = gtk_application_window_new(app);
        gtk_window_set_default_size(_window, _defaultWidth, _defaultHeight);
        g_signal_connect(_window, "destroy", FunctionPointer<DestroyHandler>(CloseWindow), app);

        // Add the WebView
        _webView = webkit_web_view_new();
        gtk_container_add(_window, _webView);
        g_signal_connect(_webView, "load-changed", FunctionPointer<LoadChangedHandler>(LoadChanged), _webView);
        _settings = webkit_web_view_get_settings(_webView);

        // Show the window on the screen
        gtk_widget_show_all(_window);

        Activated?.Invoke(this);
    }

    void CloseWindow(nint widget, nint app)
    {
        Close();
    }

    void LoadChanged(nint webView, WebkitLoadEvent loadEvent, nint data)
    {
        if (loadEvent == WebkitLoadEvent.WEBKIT_LOAD_FINISHED)
        {
            JSInterop.Initialize(webView);

            Loaded?.Invoke(this);
        }
    }

    public Action<WebWindow>? Activated;
    public Action<WebWindow>? Loaded;
    public Action<WebWindow>? Closing;
    public Action<WebWindow>? Closed;

    public void LoadHTML(string html)
    {
        webkit_web_view_load_html(_webView, html, null);
    }

    public int Run()
    {
        var status = g_application_run(_app, 0, nint.Zero);
        g_object_unref(_app);

        return status;
    }

    public void Close()
    {
        Closing?.Invoke(this);
        gtk_window_close(_window);
        Closed?.Invoke(this);
    }

    public bool IsFullscreen
    {
        get
        {
            var gdkWindow = gtk_widget_get_window(_window);
            var state = gdk_window_get_state(gdkWindow);

            return state.HasFlag(GdkWindowState.GDK_WINDOW_STATE_FULLSCREEN);
        }
    }

    public void EnterFullscreen()
    {
        if (!IsFullscreen)
        {
            gtk_window_fullscreen(_window);
        }
    }

    public void LeaveFullscreen()
    {
        if (IsFullscreen)
        {
            gtk_window_unfullscreen(_window);
        }
    }

    public bool DeveloperExtrasAreEnabled
    {
        get => webkit_settings_get_enable_developer_extras(_settings);
        set=> webkit_settings_set_enable_developer_extras(_settings, true);
    }
}