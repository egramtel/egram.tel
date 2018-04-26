using Egram.Components;
using Egram.Components.App;
using Egram.Components.Authorization;
using Egram.Components.Chatting;
using Egram.Components.Main;
using Egram.Components.Navigation;
using Egram.Components.Workarea;
using Microsoft.Extensions.DependencyInjection;

namespace Egram.Registry
{
    public static class ContextRegistry
    {
        public static void AddContext(this IServiceCollection services)
        {
            services.AddScoped<ApplicationContext>();
            
            services.AddScoped<AuthorizationContext>();
            services.AddScoped<AuthorizationContextFactory>();
            
            services.AddScoped<MainContext>();
            services.AddScoped<MainContextFactory>();
            
            services.AddScoped<ExplorerContext>();
            services.AddScoped<ExplorerContextFactory>();

            services.AddScoped<AggregatorContext>();
            services.AddScoped<AggregatorContextFactory>();
            
            services.AddScoped<CatalogContext>();
            services.AddScoped<CatalogContextFactory>();
            
            services.AddScoped<WorkareaContext>();
            services.AddScoped<WorkareaContextFactory>();
            
            services.AddScoped<ChatContext>();
            services.AddScoped<ChatContextFactory>();

            services.AddScoped<ToolbarContext>();
            services.AddScoped<ToolbarContextFactory>();
        }
    }
}