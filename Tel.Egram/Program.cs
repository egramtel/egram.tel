using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Application;
using Tel.Egram.Gui;
using Tel.Egram.Registry;

namespace Tel.Egram
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            
            var provider = services.BuildServiceProvider();
            Run(provider);
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddUtils();
            services.AddApplication();
            services.AddAuthentication();
            services.AddWorkspace();
            services.AddNavigation();
            services.AddExplorer();
            services.AddContent();
            services.AddMessenger();
            services.AddCatalog();
        }

        static void Run(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationContext>();
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