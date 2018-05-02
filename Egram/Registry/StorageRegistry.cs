using Egram.Components.Chatting;
using Egram.Components.Graphics;
using Egram.Components.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Egram.Registry
{
    public static class StorageRegistry
    {
        public static void AddStorage(this IServiceCollection services)
        {
            services.AddSingleton<Storage>();
            services.AddSingleton<FileLoader>();

            services.AddScoped<ColorMaker>();
            services.AddScoped<AvatarLoader>();
        }
    }
}