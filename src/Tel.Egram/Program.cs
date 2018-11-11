using System;
using Avalonia;
using Avalonia.Gtk3;
using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using Splat;
using Tel.Egram.Components;
using Tel.Egram.Components.Application;
using Tel.Egram.Gui;
using Tel.Egram.Gui.Views.Application;
using Tel.Egram.Utils;

namespace Tel.Egram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureServices(Locator.CurrentMutable);
            Run(Locator.Current);
        }

        private static void ConfigureServices(
            IMutableDependencyResolver services)
        {
            services.AddTdLib();
            services.AddPersistance();
            services.AddServices();
            
            services.AddApplication();
            services.AddAuthentication();
            services.AddWorkspace();
            services.AddSettings();
            services.AddMessenger();
        }

        private static void Run(
            IDependencyResolver resolver)
        {
            var app = resolver.GetService<MainApplication>();
            var builder = AppBuilder.Configure(app);
            var os = builder.RuntimePlatform.GetRuntimeInfo().OperatingSystem;
            
            if (os == OperatingSystemType.OSX)
            {
                builder.UseAvaloniaNative(null, opt =>
                {
                    opt.MacOptions.ShowInDock = true;
                    opt.UseDeferredRendering = true;
                    opt.UseGpu = true;
                }).UseSkia();
            }
            else if (os == OperatingSystemType.Linux)
            {
                builder.UseGtk3(new Gtk3PlatformOptions
                {
                    UseDeferredRendering = true,
                    UseGpuAcceleration = true
                }).UseSkia();
            }
            else
            {
                builder.UseWin32(
                    deferredRendering: true
                ).UseSkia();
            }

            builder.UseReactiveUI();
            builder.Start<MainWindow>(() => new MainWindowModel());
        }
    }
}