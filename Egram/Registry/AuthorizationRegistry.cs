using Egram.Components.Authorization;
using Egram.Components.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Egram.Registry
{
    public static class AuthorizationRegistry
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddScoped<Authorizer>();
            services.AddSingleton(p => new Authorizer.Parameters
            {
                UseTestDc = false,
                ApiId = 111112,
                ApiHash = new byte[]{ 142, 34, 97, 121, 94, 51, 206, 139, 4, 159, 245, 26, 236, 242, 11, 171 },
                AppVersion = "0.1",
                Device = "Mac",
                SystemVersion = "0.1",
                Lang = "en",
                FilesDir = p.GetService<Storage>().TdlibDirectory
            });
        }
    }
}