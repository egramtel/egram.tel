using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Application;
using Tel.Egram.Gui;
using Tel.Egram.Gui.Views.Application;
using Tel.Egram.Registry;

namespace Tel.Egram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            
            var provider = services.BuildServiceProvider();
            Run(provider);
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
        }

        private static void Run(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationModel>();
                var app = scope.ServiceProvider.GetService<MainApplication>();
                
                AppBuilder
                    .Configure(app)
                    .UsePlatformDetect()
                    .UseReactiveUI()
                    .Start<MainWindow>(() => context);
            }
        }
    }
}