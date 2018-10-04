using System.Reactive.Concurrency;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Feeds;
using Tel.Egram.Graphics;
using Tel.Egram.Messages;
using Tel.Egram.Persistance;
using Tel.Egram.Settings;
using Tel.Egram.TdLib;
using Tel.Egram.Users;
using Tel.Egram.Utils;

namespace Tel.Egram.Registry
{
    public static class UtilsRegistry
    {
        public static void AddUtils(this IServiceCollection services)
        {
            // utils
            services.AddScoped(typeof(IFactory<>), typeof(Factory<>));
            services.AddScoped(typeof(IFactory<,>), typeof(Factory<,>));
            
            // tdlib
            services.AddScoped(_ =>
            {
                Client.Log.SetVerbosityLevel(1);
                return new Client();
            });
            services.AddScoped<Hub>();
            services.AddScoped<Dialer>();
            services.AddScoped<TdAgent>();
            
            // persistance
            services.AddScoped<IStorage, Storage>();
            services.AddScoped<IFileLoader, FileLoader>();
            services.AddScoped<IDatabaseContextFactory, DatabaseContextFactory>();
            services.AddScoped<IKeyValueStorage, KeyValueStorage>();
            
            // settings
            services.AddScoped<IProxyManager, ProxyManager>();

            // graphics
            services.AddScoped<IColorMapper, ColorMapper>();
            services.AddScoped<IAvatarCache>(p =>
            {
                var options = Options.Create(new MemoryCacheOptions
                {
                    SizeLimit = 64
                });
                return new AvatarCache(new MemoryCache(options));
            });
            services.AddScoped<IBitmapLoader, BitmapLoader>();
            services.AddScoped<IAvatarLoader, AvatarLoader>();
            
            // feeds
            services.AddScoped<IChatLoader, ChatLoader>();
            services.AddScoped<IChatUpdater, ChatUpdater>();
            services.AddScoped<IFeedLoader, FeedLoader>();
            services.AddScoped<IMessageLoader, MessageLoader>();
            
            // users
            services.AddScoped<IUserLoader, UserLoader>();
            
            // auth
            services.AddScoped<IAuthenticator, Authenticator>();
        }
    }
}