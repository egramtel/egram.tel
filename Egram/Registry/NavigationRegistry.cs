using Egram.Components.Navigation;
using Egram.Components.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace Egram.Registry
{
    public static class NavigationRegistry
    {
        public static void AddNavigation(this IServiceCollection services)
        {
            services.AddScoped<Navigator>();
            services.AddScoped<ConversationLoader>();
            services.AddScoped<SegmentInteractor>();
        }
    }
}