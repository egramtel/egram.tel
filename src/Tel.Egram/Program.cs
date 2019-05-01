using System;
using Avalonia;
using Avalonia.Platform;
using Splat;
using Tel.Egram.Application;
using Tel.Egram.Views.Application;
using Tel.Egram.Model.Application;

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
            services.AddUtils();
            services.AddTdLib();
            services.AddPersistance();
            services.AddServices();
            
            services.AddComponents();
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
            var runtime = builder.RuntimePlatform.GetRuntimeInfo();
            var model = new MainWindowModel();
            
            switch (runtime.OperatingSystem)
            {
                case OperatingSystemType.OSX:
                    builder.UseAvaloniaNative()
                        .With(new AvaloniaNativePlatformOptions
                        {
                            UseGpu = true,
                            UseDeferredRendering = true
                        })
                        .UseSkia();
                    break;
                
                case OperatingSystemType.Linux:
                    builder.UseX11()
                        .With(new X11PlatformOptions
                        {
                            UseGpu = true
                        })
                        .UseSkia();
                    break;
                
                default:
                    builder.UseWin32()
                        .With(new Win32PlatformOptions
                        {
                            UseDeferredRendering = true
                        })
                        .UseSkia();
                    break;
            }

            builder.UseReactiveUI();

            model.Activator.Activate();
            builder.Start<MainWindow>(() => model);
            model.Activator.Deactivate();
        }
    }
}