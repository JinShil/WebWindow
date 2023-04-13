using System.Runtime.InteropServices;

namespace Native;

public static class Gio
{
    public const string FilePath = "libgio-2.0.so.0";

    public enum ApplicationFlags
    {
        NONE = 0
    }

    [DllImport(FilePath)]
    public static extern int g_application_run(nint application, int argc, nint argv);

    [DllImport(FilePath)]
    public static extern void g_object_unref(nint obj);

    [DllImport(FilePath)]
    public static extern nint g_main_context_default();

    [DllImport(FilePath)]
    public static extern bool g_main_context_iteration(nint context, bool may_block);

    [DllImport(FilePath)]
    public static extern nint g_memory_input_stream_new_from_data (byte[] data, uint len, nint destroy);

    [DllImport(FilePath)]
    public static extern void g_free(nint o);
}

public static class GObject
{
    public const string FilePath = "libgobject-2.0.so.0";

    public struct GError 
    {
        public uint domain;
        public int code;
        public nint message;
    }

    [DllImport(FilePath)]
    public static extern unsafe ulong g_error_free(GError* error);

    public enum GConnectFlags : int
    {
        AFTER,  	 
        SWAPPED
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GCallback(nint instance, nint data);

    [DllImport(FilePath)]
    public static extern ulong g_signal_connect_data (nint instance, string detailed_signal, nint c_handler, nint data, nint destroy_data, GConnectFlags connect_flags);

    public static ulong g_signal_connect(nint instance, string detailed_signal, nint c_handler, nint data)
    {
        return g_signal_connect_data(instance, detailed_signal, c_handler, data, nint.Zero, (GConnectFlags)0);
    }
}

public static class Gtk
{
    public const string FilePath = "libgtk-3.so.0";

    public enum GtkWindowType : int
    {
        GTK_WINDOW_TOPLEVEL,
        GTK_WINDOW_WINDOW_POPUP
    }

    [DllImport(FilePath)]
    public static extern void gtk_init(nint argc, nint argv);

    [DllImport(FilePath)]
    public static extern nint gtk_application_new (string application_id, Gio.ApplicationFlags flags);

    [DllImport(FilePath)]
    public static extern nint gtk_window_new(GtkWindowType type);

    [DllImport(FilePath)]
    public static extern void gtk_window_set_default_size(nint window, int width, int height);

    [DllImport(FilePath)]
    public static extern void gtk_container_add(nint container, nint widget);

    [DllImport(FilePath)]
    public static extern void gtk_widget_show_all(nint widget);
}

public static class WebKit
{
    public const string FilePath = "libwebkit2gtk-4.0.so.37";

    public enum WebKitUserContentInjectedFrames
    {
        WEBKIT_USER_CONTENT_INJECT_ALL_FRAMES = 0,
        WEBKIT_USER_CONTENT_INJECT_TOP_FRAME = 1
    }

    public enum WebKitUserScriptInjectionTime
    {
        WEBKIT_USER_SCRIPT_INJECT_AT_DOCUMENT_START = 0,
        WEBKIT_USER_SCRIPT_INJECT_AT_DOCUMENT_END = 1
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WebKitURISchemeRequestCallback(nint instance, nint data);

     [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GDestroyNotify(nint data);

    [DllImport(FilePath)]
    public static extern nint webkit_web_view_new();

    [DllImport(FilePath)]
    public static extern nint webkit_web_view_new_with_context(nint context);

    [DllImport(FilePath)]
    public static extern nint webkit_web_context_get_default();

    [DllImport(FilePath)]
    public static extern nint webkit_user_script_new(string source, WebKitUserContentInjectedFrames injected_frames,
        WebKitUserScriptInjectionTime injection_time, string? allow_list, string? block_list);

    [DllImport(FilePath)]
    public static extern nint webkit_web_view_get_context(nint web_view);

    [DllImport(FilePath)]
    public static extern void webkit_user_content_manager_add_script(nint manager, nint script);

    [DllImport(FilePath)]
    public static extern void webkit_user_script_unref(nint script);

    [DllImport(FilePath)]
    public static extern bool webkit_user_content_manager_register_script_message_handler(nint manager, string name);

    [DllImport(FilePath)]
    public static extern void webkit_web_view_run_javascript (nint web_view, string script, nint cancellable, nint callback, nint user_data);

    [DllImport(FilePath)]
    public static extern void webkit_javascript_result_unref(nint js_result);

    [DllImport(FilePath)]
    public static extern nint webkit_javascript_result_get_js_value(nint js_result);

    [DllImport(FilePath)]
    public static extern nint webkit_web_view_run_javascript_finish(nint web_view, nint result, nint error);

    [DllImport(FilePath)]
    public static extern bool jsc_value_is_string(nint value);

    [DllImport(FilePath)]
    public static extern nint jsc_value_to_string(nint value);

    [DllImport(FilePath)]
    public static extern nint jsc_value_object_get_property(nint value, string name);

    [DllImport(FilePath)]
    public static extern void webkit_web_context_register_uri_scheme(nint context, string scheme, WebKitURISchemeRequestCallback callback, nint user_data, nint user_data_destroy_func);

    [DllImport(FilePath)]
    public static extern nint webkit_web_view_get_user_content_manager(nint web_view);

    [DllImport(FilePath)]
    public static extern void webkit_web_view_load_uri(nint web_view, string uri);

    [DllImport(FilePath)]
    public static extern nint webkit_uri_scheme_request_get_scheme(nint request);

    [DllImport(FilePath)]
    public static extern nint webkit_uri_scheme_request_get_uri(nint request);

    [DllImport(FilePath)]
    public static extern nint webkit_uri_scheme_request_get_path (nint request);

    [DllImport(FilePath)]
    public static extern void webkit_uri_scheme_request_finish(nint request, nint stream, long stream_length, string mime_type);

    [DllImport(FilePath)]
    public static extern nint webkit_web_view_get_settings(nint web_view);

    [DllImport(FilePath)]
    public static extern bool webkit_settings_get_enable_developer_extras(nint settings);

    [DllImport(FilePath)]
    public static extern void webkit_settings_set_enable_developer_extras (nint settings, bool enabled);

    [DllImport(FilePath)]
    public static extern void webkit_web_view_load_html (nint web_view, string content, string? base_uri);

    [DllImport(FilePath)]
    public static extern nint webkit_web_extension_get_page(nint extension, ulong pageId);

    [DllImport(FilePath)]
    public static extern void webkit_web_context_set_web_extensions_directory(nint context, string directory);

    [DllImport(FilePath)]
    public static extern nint webkit_web_page_get_dom_document(nint web_page);

    [DllImport(FilePath)]
    public static extern nint webkit_dom_document_get_body(nint dom);

    [DllImport(FilePath)]
    public static extern void webkit_dom_html_element_set_inner_html (nint self, string contents, nint error);

    [DllImport(FilePath)]
    public static extern string webkit_dom_html_element_get_inner_html(nint self);

    [DllImport(FilePath)]
    public static extern ulong webkit_web_page_get_id(nint web_page);

    [DllImport(FilePath)]
    public static extern ulong webkit_web_view_get_page_id(nint web_view);

    [DllImport(FilePath)]
    public static extern bool webkit_dom_event_target_add_event_listener(nint target, string event_name, nint handler, bool use_capture, nint user_data);
}