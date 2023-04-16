# WebWindow
A [WebkitGtk](https://webkitgtk.org/) Window for building [native AoT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) desktop applications on Linux using web technologies, C#, and .NET

## Why?
.NET Maui is not completely multi-platform; it doesn't support Linux.  Therefore, there is no Microsoft supported way to create desktop applications in C# and .NET using web technologies.

I created a [WebKit-based Blazor WebView](https://github.com/JinShil/BlazorWebView) that can be used to created [Blazor Hybrid](https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/) applications on Linux, but on low-spec ARM devices the startup time can be rather poor.  This could be fixed with native AoT, but native AoT currently generates unusable code code for anything with [Razor components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/) (including Blazor) causing the application to either segfault or otherwise not function properly.

This solution does not use Razor commponents.  Instead it simply exposes the WebKitGTK WebView's DOM with Javascript interop so DOM manipulation can be done in C#.  Because there are no Razor components, this solution can be compiled with native AoT for fast start times while still providing convenient DOM manipulation in C#.

## Run the Demonstration
```
git clone https://github.com/JinShil/WebWindow.git
cd WebWindow/WebWindow.Test
dotnet run
```

## Run the Demonstration with Native AoT
You may need to install these dependencies:
```
sudo apt-get install clang zlib1g-dev
```
Then publish the project with Native AoT.
```
cd WebWindow/WebWindow.Test
dotnet publish -r linux-x64 -c Release --self-contained=true -p:PublishAot=true -p:StripSymbols=true
```

If you get an error about not being able to find `-lstdc++` try this and then run the publish command again.
```
sudo apt install libstdc++-12-dev
```

Then run the resulting executable:
```
./bin/Release/net7.0/linux-x64/publish/WebWindow.Test
```