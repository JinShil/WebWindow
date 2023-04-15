using System.Runtime.InteropServices;

namespace WebWindow.Native;

internal static class GObject
{
    const string FilePath = "libgobject-2.0.so.0";

    public struct GError
    {
        public GError()
        { }

        public uint domain = 0;
        public int code = 0;
        public nint message = nint.Zero;
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
