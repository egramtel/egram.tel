using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Splat;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components.Application;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Messenger.Editor;
using Tel.Egram.Components.Messenger.Explorer;
using Tel.Egram.Components.Messenger.Informer;
using Tel.Egram.Components.Notifications;
using Tel.Egram.Components.Popups;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Components.Workspace.Navigation;
using Tel.Egram.Graphics;
using Tel.Egram.Gui;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Messaging.Notifications;
using Tel.Egram.Messaging.Users;
using Tel.Egram.Persistance;
using Tel.Egram.Settings;
using Tel.Egram.Utils.Platforms;
using Tel.Egram.Utils.TdLib;
using IBitmapLoader = Tel.Egram.Graphics.IBitmapLoader;
using BitmapLoader = Tel.Egram.Graphics.BitmapLoader;

namespace Tel.Egram
{
    public static class Registry
    {
        public static void AddUtils(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton<IPlatform>(Platform.GetPlatform);
        }
        
        public static void AddTdLib(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton(() =>
            {
                Client.Log.SetVerbosityLevel(1);
                return new Client();
            });

            services.RegisterLazySingleton(() =>
            {
                var client = services.GetService<Client>();
                return new Hub(client);
            });

            services.RegisterLazySingleton(() =>
            {
                var client = services.GetService<Client>();
                var hub = services.GetService<Hub>();
                return new Dialer(client, hub);
            });

            services.RegisterLazySingleton<IAgent>(() =>
            {
                var hub = services.GetService<Hub>();
                var dialer = services.GetService<Dialer>();
                return new Agent(hub, dialer);
            });
        }
        
        public static void AddPersistance(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton<IStorage>(() => new Storage());
            
            services.RegisterLazySingleton<IFileLoader>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new FileLoader(agent);
            });
            
            services.RegisterLazySingleton<IDatabaseContextFactory>(() => new DatabaseContextFactory());
            
            services.RegisterLazySingleton(() =>
            {
                var factory = services.GetService<IDatabaseContextFactory>();
                return factory.CreateDbContext();
            });
            
            services.RegisterLazySingleton<IKeyValueStorage>(() =>
            {
                var db = services.GetService<DatabaseContext>();
                return new KeyValueStorage(db);
            });
        }

        public static void AddServices(this IMutableDependencyResolver services)
        {
            // graphics
            services.RegisterLazySingleton<IColorMapper>(() => new ColorMapper());
            
            services.RegisterLazySingleton<IAvatarCache>(() =>
            {
                var options = Options.Create(new MemoryCacheOptions
                {
                    SizeLimit = 128 // maximum 128 cached bitmaps
                });
                return new AvatarCache(new MemoryCache(options));
            });
            
            services.RegisterLazySingleton<IBitmapLoader>(() =>
            {
                var fileLoader = services.GetService<IFileLoader>();
                return new BitmapLoader(fileLoader);
            });
            
            services.RegisterLazySingleton<IAvatarLoader>(() =>
            {
                var fileLoader = services.GetService<IFileLoader>();
                var avatarCache = services.GetService<IAvatarCache>();
                var colorMapper = services.GetService<IColorMapper>();
                
                return new AvatarLoader(
                    fileLoader,
                    avatarCache,
                    colorMapper);
            });
            
            // chats
            services.RegisterLazySingleton<IChatLoader>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new ChatLoader(agent);
            });
            
            services.RegisterLazySingleton<IChatUpdater>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new ChatUpdater(agent);
            });
            
            services.RegisterLazySingleton<IFeedLoader>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new FeedLoader(agent);
            });
            
            // messages
            services.RegisterLazySingleton<IMessageLoader>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new MessageLoader(agent);
            });
            services.RegisterLazySingleton<IMessageSender>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new MessageSender(agent);
            });
            
            // notifications
            services.RegisterLazySingleton<INotificationSource>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new NotificationSource(agent);
            });
            
            // users
            services.RegisterLazySingleton<IUserLoader>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new UserLoader(agent);
            });
            
            // auth
            services.RegisterLazySingleton<IAuthenticator>(() =>
            {
                var agent = services.GetService<IAgent>();
                var storage = services.GetService<IStorage>();
                return new Authenticator(agent, storage);
            });
            
            // settings
            services.RegisterLazySingleton<IProxyManager>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new ProxyManager(agent);
            });
        }

        public static void AddComponents(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton<INotificationController>(() => new NotificationController());
            services.RegisterLazySingleton<IPopupController>(() => new PopupController());
        }
        
        public static void AddApplication(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton(() =>
            {
                var application = new MainApplication();
                
                application.Initializing += (sender, args) =>
                {
                    var db = services.GetService<DatabaseContext>();
                    db.Database.Migrate();
                
                    var hub = services.GetService<Hub>();
                    var task = Task.Factory.StartNew(
                        () => hub.Start(),
                        TaskCreationOptions.LongRunning);
                    
                    task.ContinueWith(t =>
                    {
                        var exception = t.Exception;
                        if (exception != null)
                        {
                            // TODO: handle exception and shutdown
                        }
                    });
                };

                application.Disposing += (sender, args) =>
                {
                    var hub = services.GetService<Hub>();
                    hub.Stop();
                };
                
                return application;
            });
        }
        
        public static void AddAuthentication(this IMutableDependencyResolver services)
        {
            //
        }
        
        public static void AddMessenger(this IMutableDependencyResolver services)
        {
            // catalog
            services.RegisterLazySingleton<ICatalogProvider>(() =>
            {
                var chatLoader = services.GetService<IChatLoader>();
                var chatUpdater = services.GetService<IChatUpdater>();
                
                return new CatalogProvider(
                    chatLoader,
                    chatUpdater);
            });
            
            // messenger
            services.RegisterLazySingleton<IAvatarManager>(() =>
            {
                var avatarLoader = services.GetService<IAvatarLoader>();
                return new AvatarManager(avatarLoader);
            });
            
            services.RegisterLazySingleton<IMessageModelFactory>(() =>
            {
                return new MessageModelFactory();
            });
            
            services.RegisterLazySingleton<IMessageManager>(() =>
            {
                var messageLoader = services.GetService<IMessageLoader>();
                var messageFactory = services.GetService<IMessageModelFactory>();
                return new MessageManager(messageLoader, messageFactory);
            });
        }
        
        public static void AddSettings(this IMutableDependencyResolver services)
        {
            //
        }
        
        public static void AddWorkspace(this IMutableDependencyResolver services)
        {
            //
        }
    }
}