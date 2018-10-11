using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Authentication;

namespace Tel.Egram.Registry
{
    public static class AuthenticationRegistry
    {
        public static void AddAuthentication(this IServiceCollection services)
        {
            services.AddTransient<AuthenticationModel>();
        }
    }
}