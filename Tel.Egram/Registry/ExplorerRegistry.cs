using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Content;
using Tel.Egram.Components.Content.Direct;
using Tel.Egram.Components.Content.Home;
using Tel.Egram.Components.Explorer;
using Tel.Egram.Utils;

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