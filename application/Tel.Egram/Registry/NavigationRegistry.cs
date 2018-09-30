using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Navigation;

namespace Tel.Egram.Registry
{
    public static class NavigationRegistry
    {
        public static void AddNavigation(this IServiceCollection services)
        {
            services.AddTransient<NavigationContext>();
        }
    }
}