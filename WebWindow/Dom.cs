using System.Runtime.InteropServices;

namespace WebWindow;

public class Dom
{
    delegate void void_nint_nint_nint(nint arg0, nint arg1, nint arg2);

    class Future
    {
        nint _context = g_main_context_default();
        bool _done = false;
        public string? ErrorMessage { get; private set;} = null;

        public unsafe void Finish(nint webView, nint jResult, nint data)
        {
            GError* err;
            var jsResult = webkit_web_view_run_javascript_finish(webView, jResult, new nint(&err));
            if (jsResult == nint.Zero)
            {
                ErrorMessage = Marshal.PtrToStringAuto(err->message);
                g_error_free(err);
            }
            else
            {
                webkit_javascript_result_unref(jsResult);
            }

            _done = true;
        }

        public void Wait()
        {
            while(!_done)
            {
                g_main_context_iteration(_context, true);
            }

            if (ErrorMessage is not null)
            {
                // throw new Exception(ErrorMessage);
                Error.WriteLine(ErrorMessage);
            }
        }
    }

    class Future<T>
    {
        nint _context = g_main_context_default();
        bool _done = false;
        public string? ErrorMessage { get; private set;} = null;
        T? _result = default(T);

        public unsafe void Finish(nint webView, nint jResult, nint data)
        {
            GError* err;
            var jsResult = webkit_web_view_run_javascript_finish(webView, jResult, new nint(&err));
            if (jsResult == nint.Zero)
            {
                ErrorMessage = Marshal.PtrToStringAuto(err->message);
                g_error_free(err);
            }
            else
            {
                var jsValue = webkit_javascript_result_get_js_value(jsResult);
                
                if (jsValue == nint.Zero)
                {
                    _result = default(T?);
                }
                else if (typeof(T) == typeof(string))
                {
                    var p = jsc_value_to_string(jsValue);
                    _result = (T?)(object?)Marshal.PtrToStringAuto(p);
                }

                webkit_javascript_result_unref(jsResult);
            }

            _done = true;
        }

        public void Wait()
        {
            while(!_done)
            {
                g_main_context_iteration(_context, true);
            }

            if (ErrorMessage is not null)
            {
                // throw new Exception(ErrorMessage);
                Error.WriteLine(ErrorMessage);
            }
        }

        public T? Result
        {
            get
            {
                Wait();

                return _result;
            }
        }
    }

    public static void Initialize(nint webView)
    {
        _webView = webView;
        _context = g_main_context_default();

        var webViewContentManager = webkit_web_view_get_user_content_manager(webView);
        webkit_user_content_manager_register_script_message_handler(webViewContentManager, "webview");
        g_signal_connect(webViewContentManager, "script-message-received::webview", Marshal.GetFunctionPointerForDelegate<void_nint_nint_nint>(HandleWebMessage), webView);

        // Serializes javascript events to JSON.
        Emit(
            """
            function stringifyEvent(e)
            {
                const obj = {};
                for (let k in e)
                {
                    let v = e[k];
                    if (v instanceof Node)
                    {
                        v = v.id;
                    }
                    else if (v instanceof Window)
                    {
                        v = "window";
                    }
                    
                    obj[k] = v;
                }

                return JSON.stringify(obj);
            }
            """
        );
    }

    const string _handlerPropertyName = "dotnetMethod";
    static nint _context;
    static nint _webView;

    static Dictionary<string, object> Handlers = new();

    static void HandleWebMessage(nint contentManager, nint jsResult, nint webView)
    {
        var jsValue = webkit_javascript_result_get_js_value(jsResult);

        if (jsc_value_is_string(jsValue)) 
        {
            var p = jsc_value_to_string(jsValue);
            var s = Marshal.PtrToStringAuto(p);
            if (s is not null)
            {
                var op = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var e = JsonSerializer.Deserialize<Event>(s, op);
                if (e is null)
                {
                    Error.WriteLine($"Could not deserialize event.");
                }
                else
                {
                    var m = e.DotNetMethod;
                    if (m is not null)
                    {
                        if (!Handlers.TryGetValue(m, out object? handler))
                        {
                            Error.WriteLine($"Handler \"{m}\" was not registered.");
                        }
                        else
                        {
                            if (handler is Action<MouseEvent>)
                            {
                                var me = JsonSerializer.Deserialize<MouseEvent>(s, op);
                                ((Action<MouseEvent>)handler)(me!);
                            }
                            else if (handler is Action<UIEvent>)
                            {
                                var ue = JsonSerializer.Deserialize<UIEvent>(s, op);
                                ((Action<UIEvent>)handler)(ue!);
                            }
                            else if (handler is Action<Event>)
                            {
                                ((Action<Event>)handler)(e);
                            }
                            else
                            {
                                Error.WriteLine("Event was not implemented");
                            }

                        }
                    }
                }
            }
        }

        webkit_javascript_result_unref(jsResult);
    }

    public static T Read<T>(string js)
    {
        var result = ReadAsync<T>(js).Result;

        if (default(T) is not null)
        {
            if (result is null)
            {
                throw new Exception("Returned null, when expecting not null");
            }
        }

        return result!;
    }

    public static void Write(string js)
    {
        WriteAsync(js).Wait();
    }

    static Future<T> ReadAsync<T>(string js)
    {
        var f = new Future<T>();
        webkit_web_view_run_javascript(_webView, js, nint.Zero, Marshal.GetFunctionPointerForDelegate<void_nint_nint_nint>(f.Finish), nint.Zero);
        return f;
    }

    static Future WriteAsync(string js)
    {
        var f = new Future();
        webkit_web_view_run_javascript(_webView, js, nint.Zero, Marshal.GetFunctionPointerForDelegate<void_nint_nint_nint>(f.Finish), nint.Zero);
        return f;
    }

    static Future EmitAsync(string js)
    {
        var f = new Future();
        webkit_web_view_run_javascript(_webView, js, nint.Zero, Marshal.GetFunctionPointerForDelegate<void_nint_nint_nint>(f.Finish), nint.Zero);
        return f;
    }

    public static void Emit(string js)
    {
        EmitAsync(js).Wait();
    }

    static Document? _document;
    public static Document Document
    {
        get
        {
            if (_document is null)
            {
                _document = new Document("document");
            }

            return _document;
        }
    }

    public static void Invoke(string method, params string[] args)
    {
        Dom.Emit($"{method}({string.Join(',', args)});");
    }

    public static void AddEventListener<T>(string selector, string evt, Action<T> action, bool useCapture = false)
        where T : Event
    {
        var id = action.GetHashCode().ToString();
        var name = $"_{id}";

        if (Handlers.TryAdd(id, action))
        {
            Emit($$"""
                function {{name}}(e) 
                {
                    e.{{_handlerPropertyName}} = "{{id}}";
                    window.webkit.messageHandlers.webview.postMessage(stringifyEvent(e)); 
                }
                """);
        }

        // Make it `passive` because we don't have any way to call `preventDefault` anyway.
        Invoke($"{selector}.addEventListener", $"\"{evt}\"", name, "{ passive: true }", useCapture.ToString().ToLower());
    }

    public static void RemoveEventListener<T>(string selector, string evt, Action<T> action, bool useCapture = false)
        where T : Event
    {
        var id = action.GetHashCode().ToString();
        
        var name = $"_{id}";
        Invoke($"{selector}.removeEventListener", $"\"{evt}\"", name, useCapture.ToString().ToLower());
    }
}