using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TdLib;
using Tel.Egram.Components.Application;
using Tel.Egram.Gui;
using Tel.Egram.Persistance;

namespace Tel.Egram.Registry
{
    public static class ApplicationRegistry
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<MainApplication>();
            services.AddScoped<ApplicationContext>();
            
            services.AddScoped(provider => new MainApplication.Initializer(() =>
            {
                var db = provider.GetService<IDatabaseContextFactory>().CreateDbContext();
                db.Database.Migrate();
                
                var hub = provider.GetService<Hub>();
                Task.Run(() => hub.Start());

                return Disposable.Create(() =>
                {
                    hub.Stop();
                });
            }));
        }
    }
}