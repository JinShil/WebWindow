using System.Runtime.InteropServices;

namespace WebWindow.Native;

internal static partial class Gio
{
    const string FilePath = "libgio-2.0.so.0";

    const int G_PRIORITY_DEFAULT_IDLE = 200;

    [Flags]
    public enum GApplicationFlags
    {
        G_APPLICATION_FLAGS_NONE,
        G_APPLICATION_IS_SERVICE  =          (1 << 0),
        G_APPLICATION_IS_LAUNCHER =          (1 << 1),
        G_APPLICATION_HANDLES_OPEN =         (1 << 2),
        G_APPLICATION_HANDLES_COMMAND_LINE = (1 << 3),
        G_APPLICATION_SEND_ENVIRONMENT    =  (1 << 4),
        G_APPLICATION_NON_UNIQUE =           (1 << 5),
        G_APPLICATION_CAN_OVERRIDE_APP_ID =  (1 << 6),
        G_APPLICATION_ALLOW_REPLACEMENT   =  (1 << 7),
        G_APPLICATION_REPLACE             =  (1 << 8)

    }

    [LibraryImport(FilePath)]
    public static partial int g_application_run(nint application, int argc, nint argv);

    [LibraryImport(FilePath)]
    public static partial void g_object_unref(nint obj);

    [LibraryImport(FilePath)]
    public static partial nint g_main_context_default();

    [LibraryImport(FilePath)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool g_main_context_iteration(nint context, [MarshalAs(UnmanagedType.Bool)] bool may_block);

    [LibraryImport(FilePath)]
    public static partial nint g_memory_input_stream_new_from_data (byte[] data, uint len, nint destroy);

    [LibraryImport(FilePath)]
    public static partial void g_free(nint o);

    [LibraryImport(FilePath)]
    public static partial uint g_idle_add_full(int priority, nint function, nint data, nint notify);

    [LibraryImport(FilePath)]
    public static partial uint g_idle_add(nint function, nint data);

    [LibraryImport(FilePath)]
    public static partial void g_application_quit(nint application);

    [LibraryImport(FilePath)]
    public static partial uint g_timeout_add(uint interval, nint function, nint data);

    [LibraryImport(FilePath)]
    public static partial uint g_usleep(ulong microseconds);
}
