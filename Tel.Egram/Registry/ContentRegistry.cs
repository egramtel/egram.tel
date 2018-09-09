using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Content;

namespace Tel.Egram.Registry
{
    public static class ContentRegistry
    {
        public static void AddContent(this IServiceCollection services)
        {
            services.AddTransient<ContentMessengerContext>();
        }
    }
}