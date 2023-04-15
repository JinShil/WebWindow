using System.Runtime.InteropServices;

namespace WebWindow;

internal static class Utilities
{
    public static nint FunctionPointer<T>(T d)
        where T : notnull
    {
        return Marshal.GetFunctionPointerForDelegate<T>(d);
    }
}