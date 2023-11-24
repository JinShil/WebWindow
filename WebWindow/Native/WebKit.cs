using System.Runtime.InteropServices;

namespace WebWindow.Native;

internal static partial class WebKit
{
    public const string FilePath = "libwebkit2gtk-4.1.so.0";

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

    public enum WebkitLoadEvent
    {
        WEBKIT_LOAD_STARTED = 0,
        WEBKIT_LOAD_REDIRECTED = 1,
        WEBKIT_LOAD_COMMITTED = 2,
        WEBKIT_LOAD_FINISHED = 3
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WebKitURISchemeRequestCallback(nint instance, nint data);

     [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GDestroyNotify(nint data);

    [LibraryImport(FilePath)]
    public static partial nint webkit_web_view_new();

    [LibraryImport(FilePath)]
    public static partial nint webkit_web_view_new_with_context(nint context);

    [LibraryImport(FilePath)]
    public static partial nint webkit_web_context_get_default();

    [LibraryImport(FilePath)]
    public static partial nint webkit_user_script_new([MarshalAs(UnmanagedType.LPUTF8Str)] string source, WebKitUserContentInjectedFrames injected_frames,
        WebKitUserScriptInjectionTime injection_time, [MarshalAs(UnmanagedType.LPUTF8Str)] string? allow_list, [MarshalAs(UnmanagedType.LPUTF8Str)] string? block_list);

    [LibraryImport(FilePath)]
    public static partial nint webkit_web_view_get_context(nint web_view);

    [LibraryImport(FilePath)]
    public static partial void webkit_user_content_manager_add_script(nint manager, nint script);

    [LibraryImport(FilePath)]
    public static partial void webkit_user_script_unref(nint script);

    [LibraryImport(FilePath)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool webkit_user_content_manager_register_script_message_handler(nint manager, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(FilePath)]
    public static partial void webkit_web_view_run_javascript (nint web_view, [MarshalAs(UnmanagedType.LPUTF8Str)] string script, nint cancellable, nint callback, nint user_data);

    [LibraryImport(FilePath)]
    public static partial void webkit_javascript_result_unref(nint js_result);

    [LibraryImport(FilePath)]
    public static partial nint webkit_javascript_result_get_js_value(nint js_result);

    [LibraryImport(FilePath)]
    public static partial nint webkit_web_view_run_javascript_finish(nint web_view, nint result, nint error);

    [LibraryImport(FilePath)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool jsc_value_is_string(nint value);

    [LibraryImport(FilePath)]
    public static partial nint jsc_value_to_string(nint value);

    [LibraryImport(FilePath)]
    public static partial nint jsc_value_object_get_property(nint value, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(FilePath)]
    public static partial void webkit_web_context_register_uri_scheme(nint context, [MarshalAs(UnmanagedType.LPUTF8Str)] string scheme, WebKitURISchemeRequestCallback callback, nint user_data, nint user_data_destroy_func);

    [LibraryImport(FilePath)]
    public static partial nint webkit_web_view_get_user_content_manager(nint web_view);

    [LibraryImport(FilePath)]
    public static partial void webkit_web_view_load_uri(nint web_view, [MarshalAs(UnmanagedType.LPUTF8Str)] string uri);

    [LibraryImport(FilePath)]
    public static partial nint webkit_uri_scheme_request_get_scheme(nint request);

    [LibraryImport(FilePath)]
    public static partial nint webkit_uri_scheme_request_get_uri(nint request);

    [LibraryImport(FilePath)]
    public static partial nint webkit_uri_scheme_request_get_path (nint request);

    [LibraryImport(FilePath)]
    public static partial void webkit_uri_scheme_request_finish(nint request, nint stream, long stream_length, [MarshalAs(UnmanagedType.LPUTF8Str)] string mime_type);

    [LibraryImport(FilePath)]
    public static partial nint webkit_web_view_get_settings(nint web_view);

    [LibraryImport(FilePath)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool webkit_settings_get_enable_developer_extras(nint settings);

    [LibraryImport(FilePath)]
    public static partial void webkit_settings_set_enable_developer_extras (nint settings, [MarshalAs(UnmanagedType.Bool)] bool enabled);

    [LibraryImport(FilePath)]
    public static partial void webkit_web_view_load_html (nint web_view, [MarshalAs(UnmanagedType.LPUTF8Str)] string content, [MarshalAs(UnmanagedType.LPUTF8Str)] string? base_uri);

    [LibraryImport(FilePath)]
    public static partial nint webkit_web_extension_get_page(nint extension, ulong pageId);

    [LibraryImport(FilePath)]
    public static partial void webkit_web_context_set_web_extensions_directory(nint context, [MarshalAs(UnmanagedType.LPUTF8Str)] string directory);

    [LibraryImport(FilePath)]
    public static partial ulong webkit_web_page_get_id(nint web_page);

    [LibraryImport(FilePath)]
    public static partial ulong webkit_web_view_get_page_id(nint web_view);

    [LibraryImport(FilePath)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool webkit_dom_event_target_add_event_listener(nint target, [MarshalAs(UnmanagedType.LPUTF8Str)] string event_name, nint handler, [MarshalAs(UnmanagedType.Bool)] bool use_capture, nint user_data);

    [LibraryImport(FilePath)]
    public static partial void webkit_web_view_terminate_web_process(nint web_view);

    [LibraryImport(FilePath)]
    public static partial void webkit_web_view_try_close (nint web_view);
}
