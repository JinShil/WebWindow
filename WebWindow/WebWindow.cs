using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Concurrent;

namespace WebWindow;

public class WebWindow
{
    delegate bool DeleteHandler(nint widget, nint evt, nint data);
    delegate void ActivateHandler(nint app, nint data);
    delegate void LoadChangedHandler(nint arg0, WebkitLoadEvent arg1, nint data);
    delegate bool ContextMenuHandler(nint webView, nint contextMenu, nint evt, nint hitTestResult, nint data);
    delegate bool TimeoutHandler(nint data);

    // Had to do this to prevent handlers from being garbage collected before
    // being called.
    static ConcurrentBag<WebWindow> _windows = new();

    public WebWindow(int width = 1024, int height = 768)
    {
        _deleteHandler = new(Delete);
        _timeoutHandler = new (Timeout);
        _loadChangedHandler = new(LoadChanged);
        _onContextMenuHandler = new(OnContextMenu);
        _activateHandler = new(Activate);

        _windows.Add(this);

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

    readonly ActivateHandler _activateHandler;
    void Activate(nint app, nint data)
    {
        // Create the parent window
        _window = gtk_application_window_new(app);
        gtk_window_set_default_size(_window, _defaultWidth, _defaultHeight);
        
        g_signal_connect(_window, "delete-event", FunctionPointer<DeleteHandler>(_deleteHandler), nint.Zero);

        // Add the WebView
        _webView = webkit_web_view_new();
        gtk_container_add(_window, _webView);
        g_signal_connect(_webView, "context-menu", FunctionPointer<ContextMenuHandler>(_onContextMenuHandler), _webView);
        g_signal_connect(_webView, "load-changed", FunctionPointer<LoadChangedHandler>(_loadChangedHandler), _webView);
        _settings = webkit_web_view_get_settings(_webView);

        // Show the window on the screen
        gtk_widget_show_all(_window);

        Activated?.Invoke(this);
    }

    readonly DeleteHandler _deleteHandler;
    bool Delete(nint widget, nint evt, nint data)
    {
        Close();

        return true;
    }

    readonly LoadChangedHandler _loadChangedHandler;
    void LoadChanged(nint webView, WebkitLoadEvent loadEvent, nint data)
    {
        if (loadEvent == WebkitLoadEvent.WEBKIT_LOAD_FINISHED)
        {
            JSInterop.Initialize(webView);

            Loaded?.Invoke(this);
        }
    }

    readonly ContextMenuHandler _onContextMenuHandler;
    bool OnContextMenu(nint webView, nint contextMenu, nint evt, nint hitTestResult, nint data)
    {
        if (ContextMenu is null)
        {
            return false;
        }

        return ContextMenu.Invoke(this);
    }

    public event Action<WebWindow>? Activated;
    public event Action<WebWindow>? Loaded;
    public event Action<WebWindow>? Closing;
    public event Func<WebWindow, bool>? ContextMenu;

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

        g_application_quit(_app);
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

    bool Timeout(nint data)
    {
        var h = _timeoutHandlers;
        _timeoutHandlers = new();

        foreach(var a in h)
        {
            a();
        }

        return false;
    }

    readonly TimeoutHandler _timeoutHandler;
    List<Action> _timeoutHandlers = new();
    public void InvokeAsync(Action f)
    {
        _timeoutHandlers.Add(f);
        g_timeout_add(0, FunctionPointer<TimeoutHandler>(_timeoutHandler), nint.Zero);
    }
}