using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using System;

namespace IotProject
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "consoleMode")
            {
                ConsoleMode consoleMode = new ConsoleMode();
                consoleMode.Start();

                Console.ReadKey();
            }
            else
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();
    }
}
