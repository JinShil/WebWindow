using System;
using System.IO;
using System.Reflection;
using static System.Console;

namespace WebWindow.Test;

static class Program
{
    static int Main()
    {
        _webWindow = new WebWindow(800, 480);
        _webWindow.Activated += Activated;
        _webWindow.Loaded += Loaded;

        return _webWindow.Run();
    }

    static WebWindow _webWindow = default!;
    static HTMLSpanElement rangeValue = default!;
    static Document document = default!;

    static void Activated(WebWindow webWindow)
    {
        try
        {
            _webWindow.DeveloperExtrasAreEnabled = true;

            // Load the initial HTML content
            var asm = Assembly.GetExecutingAssembly();
            string resourceName = $"{asm.GetName().Name}.index.html";
            var stream = asm.GetManifestResourceStream(resourceName);
            if (stream is null)
            {
                // throw new Exception($"Could not locate embedded resource \"{resourceName}\"");
                Error.WriteLine($"Could not locate embedded resource \"{resourceName}\"");
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
            p1.InnerText = $"Startup Time:  {(DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime)}";

            var fsButton = document.GetElementById<HTMLButtonElement>("fs_button");
            fsButton.Click += ToggleFullscreen;

            var closeButton = document.GetElementById<HTMLButtonElement>("close_button");
            closeButton.Click += CloseWindow;
            
            // System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            // sw.Start();
            // for(int i = 0; i < 100000; i++)
            // {
            //     p1.InnerHTML = i.ToString();
            // }
            // p1.InnerHTML = sw.ElapsedMilliseconds.ToString();

            rangeValue = document.GetElementById<HTMLSpanElement>("range_value");
            rangeValue.InnerText = range1.Value;
        }
        catch(Exception ex)
        {
            Error.WriteLine(ex.Message);
        }
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