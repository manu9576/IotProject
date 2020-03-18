using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using System;
using System.Threading;

namespace IotProject
{
    class Program
    {
        static ConsoleMode consoleMode;
        static ServiceMode serviceMode;

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "consoleMode")
            {
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                consoleMode = new ConsoleMode();
                consoleMode.Start();

                SpinWait.SpinUntil(() => !consoleMode.IsRunning);

                ConsoleMode.WriteLCD("This is the end....");
            }
            else if (args.Length > 0 && args[0] == "serviceMode")
            {
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                serviceMode = new ServiceMode();
                serviceMode.Start();

                SpinWait.SpinUntil(() => !serviceMode.IsRunning);

                ServiceMode.WriteLCD("This is the end....");
            }
            else
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            consoleMode?.Stop();
            serviceMode?.Stop();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();
    }
}
