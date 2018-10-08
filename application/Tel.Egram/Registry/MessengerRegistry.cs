using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Messenger.Catalog;

namespace Tel.Egram.Registry
{
    public static class MessengerRegistry
    {
        public static void AddMessenger(this IServiceCollection services)
        {
            services.AddTransient<ExplorerContext>();
            
            services.AddScoped<ICatalogProvider, CatalogProvider>();
            services.AddTransient<CatalogContext>();
            
            services.AddScoped<AggregateMessengerContext>();
            services.AddScoped<ChatMessengerContext>();
        }
    }
}