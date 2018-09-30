using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Explorer;

namespace Tel.Egram.Registry
{
    public static class ExplorerRegistry
    {
        public static void AddExplorer(this IServiceCollection services)
        {   
            services.AddTransient<ExplorerContext>();
        }
    }
}