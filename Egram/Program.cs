using System;
using Avalonia;
using Egram.Components.App;
using Egram.Gui;
using Egram.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Egram
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
            // services
            services.AddLog();
            services.AddStorage();
            services.AddTDLib();
            services.AddAuthorization();
            services.AddNavigation();
            services.AddChatting();

            // context
            services.AddContext();
            services.AddSingleton<BackgroundWorker>();
            services.AddSingleton<App>();
        }

        static void Run(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var app = scope.ServiceProvider.GetService<App>();
                var context = scope.ServiceProvider.GetService<ApplicationContext>();
                
                AppBuilder
                    .Configure(app)
                    .UsePlatformDetect()
                    .UseReactiveUI()
                    .Start<MainWindow>(() => context);
            }
        }
    }
}