using System.Reactive.Concurrency;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReactiveUI;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Messaging.Users;
using Tel.Egram.Persistance;
using Tel.Egram.Settings;
using Tel.Egram.Utils;
using Tel.Egram.Utils.TdLib;

namespace Tel.Egram.Registry
{
    public static class ServiceRegistry
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
            services.AddScoped<IAgent, Agent>();
            
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
                    SizeLimit = 128 // maximum 128 cached bitmaps
                });
                return new AvatarCache(new MemoryCache(options));
            });
            services.AddScoped<IBitmapLoader, BitmapLoader>();
            services.AddScoped<IAvatarLoader, AvatarLoader>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            // feeds
            services.AddScoped<IChatLoader, ChatLoader>();
            services.AddScoped<IChatUpdater, ChatUpdater>();
            services.AddScoped<IFeedLoader, FeedLoader>();
            
            // messages
            services.AddScoped<IMessageLoader, MessageLoader>();
            
            // users
            services.AddScoped<IUserLoader, UserLoader>();
            
            // auth
            services.AddScoped<IAuthenticator, Authenticator>();
        }
    }
}