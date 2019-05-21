using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using Manager.ViewModels;
using Manager.Views;
using Itofinity.Refit.Cli.Utils;
using System.Collections.Generic;

namespace Manager
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args){
            CliBuilder<object>
                .Build(
                    "Itofinity Appveyor CLI",
                    ".NET Core powered CLI for AppVeyor.",
                    new List<string>() { "extensions" },
                    "*.dll")
                .Execute(args);
            //BuildAvaloniaApp().Start(AppMain, args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();

        // Your application's entry point. Here you can initialize your MVVM framework, DI
        // container, etc.
        private static void AppMain(Application app, string[] args)
        {
            var window = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            app.Run(window);
        }
    }
}
