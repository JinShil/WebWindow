using System.Runtime.InteropServices;

namespace WebWindow.Native;

internal static class Gtk
{
    const string FilePath = "libgtk-3.so.0";

    public enum GtkWindowType : int
    {
        GTK_WINDOW_TOPLEVEL,
        GTK_WINDOW_WINDOW_POPUP
    }

    [Flags]
    public enum GdkWindowState : int
    {
        GDK_WINDOW_STATE_WITHDRAWN        = 1 << 0,
        GDK_WINDOW_STATE_ICONIFIED        = 1 << 1,
        GDK_WINDOW_STATE_MAXIMIZED        = 1 << 2,
        GDK_WINDOW_STATE_STICKY           = 1 << 3,
        GDK_WINDOW_STATE_FULLSCREEN       = 1 << 4,
        GDK_WINDOW_STATE_ABOVE            = 1 << 5,
        GDK_WINDOW_STATE_BELOW            = 1 << 6,
        GDK_WINDOW_STATE_FOCUSED          = 1 << 7,
        GDK_WINDOW_STATE_TILED            = 1 << 8,
        GDK_WINDOW_STATE_TOP_TILED        = 1 << 9,
        GDK_WINDOW_STATE_TOP_RESIZABLE    = 1 << 10,
        GDK_WINDOW_STATE_RIGHT_TILED      = 1 << 11,
        GDK_WINDOW_STATE_RIGHT_RESIZABLE  = 1 << 12,
        GDK_WINDOW_STATE_BOTTOM_TILED     = 1 << 13,
        GDK_WINDOW_STATE_BOTTOM_RESIZABLE = 1 << 14,
        GDK_WINDOW_STATE_LEFT_TILED       = 1 << 15,
        GDK_WINDOW_STATE_LEFT_RESIZABLE   = 1 << 16
    }

    [DllImport(FilePath)]
    public static extern void gtk_init(nint argc, nint argv);

    [DllImport(FilePath)]
    public static extern nint gtk_application_new (string application_id, Gio.GApplicationFlags flags);

    [DllImport(FilePath)]
    public static extern nint gtk_application_window_new(nint app);

    [DllImport(FilePath)]
    public static extern nint gtk_window_new(GtkWindowType type);

    [DllImport(FilePath)]
    public static extern void gtk_window_set_default_size(nint window, int width, int height);

    [DllImport(FilePath)]
    public static extern void gtk_container_add(nint container, nint widget);

    [DllImport(FilePath)]
    public static extern void gtk_widget_show_all(nint widget);

    [DllImport(FilePath)]
    public static extern void gtk_window_fullscreen(nint window);

    [DllImport(FilePath)]
    public static extern void gtk_window_unfullscreen(nint window);

    [DllImport(FilePath)]
    public static extern void gtk_main();

    [DllImport(FilePath)]
    public static extern void gtk_main_quit();

    [DllImport(FilePath)]
    public static extern void gtk_window_close(nint window);

    [DllImport(FilePath)]
    public static extern nint gtk_widget_get_window(nint widget);

    [DllImport(FilePath)]
    public static extern GdkWindowState gdk_window_get_state(nint window);

    [DllImport(FilePath)]
    public static extern void gdk_threads_init();

    [DllImport(FilePath)]
    public static extern void gdk_threads_enter();

    [DllImport(FilePath)]
    public static extern void gdk_threads_leave();

    [DllImport(FilePath)]
    public static extern bool gtk_events_pending();

    [DllImport(FilePath)]
    public static extern void gtk_main_iteration();
}