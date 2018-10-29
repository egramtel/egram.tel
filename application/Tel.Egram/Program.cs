using System;
using Avalonia;
using Avalonia.Gtk3;
using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components;
using Tel.Egram.Components.Application;
using Tel.Egram.Gui;
using Tel.Egram.Gui.Views.Application;
using Tel.Egram.Models.Application;
using Tel.Egram.Utils;

namespace Tel.Egram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            
            var provider = services.BuildServiceProvider();
            Run(services, provider);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddUtils();
            services.AddServices();
            services.AddApplication();
            services.AddPopup();
            services.AddAuthentication();
            services.AddWorkspace();
            services.AddSettings();
            services.AddMessenger();
            services.AddReflection();
        }

        private static void Run(ServiceCollection services, IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                IController<MainWindowModel> controller = null;
                
                ControllerActivator.Instance.SetServiceCollection(services);
                ControllerActivator.Instance.SetServiceProvider(scope.ServiceProvider);
                ControllerActivator.Instance.Activate(ref controller);
                
                var app = scope.ServiceProvider.GetService<MainApplication>();
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
                    builder.UsePlatformDetect();
                }

                builder.UseReactiveUI();
                builder.Start<MainWindow>(() => controller.Model);

                ControllerActivator.Instance.Deactivate(ref controller);
            }
        }
    }
}