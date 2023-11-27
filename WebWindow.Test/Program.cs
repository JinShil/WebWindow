using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using WebWindow.Dom;
using static System.Console;

namespace WebWindow.Test;

static class Program
{
    static int Main()
    {
        _webWindow = new WebWindow(800, 480);
        _webWindow.Activated += Activated;
        _webWindow.Loaded += Loaded;
        _webWindow.Closing += Closing;

        return _webWindow.Run();
    }

    static WebWindow _webWindow = default!;
    static HTMLSpanElement rangeValue = default!;
    static Document document = default!;

    static void Activated(WebWindow webWindow)
    {
        try
        {
            // Debug builds should show a context menu, and...
            #if !DEBUG
            _webWindow.ContextMenu += (ww) => true;
            #endif

            // ... debug builds should also show the developer tools in the context menu
            #if DEBUG
            _webWindow.DeveloperExtrasAreEnabled = true;
            #endif

            // Disable hardware acceleration for now until Raspberry Pi Bookworm and virtualbox are fixed
            _webWindow.HardwareAccelerationIsEnabled = false;

            // Load the initial HTML content
            var asm = Assembly.GetExecutingAssembly();
            string resourceName = $"{asm.GetName().Name}.index.html";
            var stream = asm.GetManifestResourceStream(resourceName);
            if (stream is null)
            {
                throw new Exception($"Could not locate embedded resource \"{resourceName}\"");
            }
            else
            {
                using (var sr = new StreamReader(stream))
                {
                    _webWindow.LoadHTML(sr.ReadToEnd());
                }
            }
        }
        catch(Exception ex)
        {
            Error.WriteLine(ex.Message);
        }
    }

    static void Loaded(WebWindow webWindow)
    {
        try
        {
            // The content has been loaded. Wire things up
            document = new Document();
                
            var range1 = document.GetElementById<HTMLInputElement>("range1");
            range1.Input += OnInput;

            var p1 = document.GetElementById<HTMLParagraphElement>("p1");
            p1.InnerText = $"Startup Time:  {DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime}";

            var fsButton = document.GetElementById<HTMLButtonElement>("fs_button");
            fsButton.Click += ToggleFullscreen;

            var closeButton = document.GetElementById<HTMLButtonElement>("close_button");
            closeButton.Click += CloseWindow;

            var r = document.GetElementById<HTMLDivElement>("r");
            var g = document.GetElementById<HTMLDivElement>("g");
            var b = document.GetElementById<HTMLDivElement>("b");
            var rand = new Random();

            var updateColors = () => {};
            updateColors = () =>
            {
                try
                {
                    r.Style.Background = $"rgb({(byte)rand.Next()}, {(byte)rand.Next()}, {(byte)rand.Next()})";
                    g.Style.Background = $"rgb({(byte)rand.Next()}, {(byte)rand.Next()}, {(byte)rand.Next()})";
                    b.Style.Background = $"rgb({(byte)rand.Next()}, {(byte)rand.Next()}, {(byte)rand.Next()})";
                    _webWindow.InvokeAsync(updateColors);
                }
                catch(Exception ex)
                {
                    Error.WriteLine(ex.Message);
                }
            };

            var update = () => {};
            update = () =>
            {
                p1.InnerText = $"Run Time:  {DateTime.Now - Process.GetCurrentProcess().StartTime}";
                _webWindow.InvokeAsync(update);
            };

            _webWindow.InvokeAsync(updateColors);
            // _webWindow.InvokeAsync(update);
            

            rangeValue = document.GetElementById<HTMLSpanElement>("range_value");
            rangeValue.InnerText = range1.Value;            
        }
        catch(Exception ex)
        {
            Error.WriteLine(ex.Message);
        }
    }

    static void Closing(WebWindow window)
    {

    }

    static void CloseWindow(HTMLButtonElement el, MouseEvent e)
    {
        _webWindow.Close();
    }

    static void ToggleFullscreen(HTMLButtonElement el, MouseEvent e)
    {
        if (_webWindow.IsFullscreen)
        {
            _webWindow.LeaveFullscreen();
        }
        else
        {
            _webWindow.EnterFullscreen();
        }
    }

    static void OnInput(HTMLInputElement el, Event e)
    {
        rangeValue.InnerText = el.Value;
    }
}