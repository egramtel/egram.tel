using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Workspace;

namespace Tel.Egram.Registry
{
    public static class WorkspaceRegistry
    {
        public static void AddWorkspace(this IServiceCollection services)
        {
            services.AddTransient<NavigationModel>();
            services.AddTransient<WorkspaceModel>();
        }
    }
}