# WebWindow
A [WebkitGtk](https://webkitgtk.org/) Window for building [Native AoT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) desktop applications on Linux using web technologies, C#, and .NET

## Why?
.NET Maui is not completely multi-platform; it doesn't support Linux.  Therefore, there is no Microsoft supported way to create desktop applications in C# and .NET using web technologies.

I created a WebKit-based Blazor WebView that can be used to created [Blazor Hybrid](https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/) applications on Linux, but on low-spec ARM devices the startup time can be rather poor.  This could be fixed with Native AoT, but Microsoft it currently generates unusable code code for anything with [Razor components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/) (including Blazor) causing the application to either segfault or otherwise not function properly.

This solution does not use Razor commponents.  Instead it simply exposes the WebKitGTK WebView's DOM to C# so DOM manipulation can be done is C#.  Because there are not Razor components, this solution can be compiled with native AoT for fast start times while still providing convenient DOM manipulation in C#.

## Run the Demonstration
```
git clone https://github.com/JinShil/WebWindow.git
cd WebWindow.Test
dotnet run
```