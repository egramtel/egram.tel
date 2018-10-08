using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Workspace;

namespace Tel.Egram.Registry
{
    public static class WorkspaceRegistry
    {
        public static void AddWorkspace(this IServiceCollection services)
        {
            services.AddTransient<NavigationContext>();
            
            services.AddTransient<ContentMessengerContext>();
            services.AddTransient<WorkspaceContext>();
        }
    }
}