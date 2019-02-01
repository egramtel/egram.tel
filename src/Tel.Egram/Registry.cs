using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Splat;
using TdLib;
using Tel.Egram.Application;
using Tel.Egram.Model.Messenger.Catalog;
using Tel.Egram.Model.Messenger.Explorer;
using Tel.Egram.Model.Messenger.Explorer.Factories;
using Tel.Egram.Model.Notifications;
using Tel.Egram.Model.Popups;
using Tel.Egram.Services.Authentication;
using Tel.Egram.Services.Graphics;
using Tel.Egram.Services.Graphics.Avatars;
using Tel.Egram.Services.Graphics.Previews;
using Tel.Egram.Services.Messaging.Chats;
using Tel.Egram.Services.Messaging.Messages;
using Tel.Egram.Services.Messaging.Notifications;
using Tel.Egram.Services.Messaging.Users;
using Tel.Egram.Services.Persistance;
using Tel.Egram.Services.Settings;
using Tel.Egram.Services.Utils.Formatting;
using Tel.Egram.Services.Utils.Platforms;
using Tel.Egram.Services.Utils.TdLib;
using IBitmapLoader = Tel.Egram.Services.Graphics.IBitmapLoader;
using BitmapLoader = Tel.Egram.Services.Graphics.BitmapLoader;

namespace Tel.Egram
{
    public static class Registry
    {
        public static void AddUtils(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton<IPlatform>(Platform.GetPlatform);
            services.RegisterLazySingleton<IStringFormatter>(() => new StringFormatter());
        }
        
        public static void AddTdLib(this IMutableDependencyResolver services)
        {
            services.RegisterLazySingleton(() =>
            {
                var storage = services.GetService<IStorage>();
                
                Client.Log.SetFilePath(Path.Combine(storage.LogDirectory, "tdlib.log"));
                Client.Log.SetMaxFileSize(1_000_000); // 1MB
                Client.Log.SetVerbosityLevel(5);
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
            services.RegisterLazySingleton<IResourceManager>(
                () => new ResourceManager(typeof(MainApplication).Assembly));
            
            services.RegisterLazySingleton<IStorage>(() => new Storage());
            
            services.RegisterLazySingleton<IFileLoader>(() =>
            {
                var agent = services.GetService<IAgent>();
                return new FileLoader(agent);
            });
            
            services.RegisterLazySingleton<IFileExplorer>(() =>
            {
                var platform = services.GetService<IPlatform>();
                return new FileExplorer(platform);
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
            
            services.RegisterLazySingleton<IBitmapLoader>(() =>
            {
                var fileLoader = services.GetService<IFileLoader>();
                return new BitmapLoader(fileLoader);
            });
            
            // avatars
            services.RegisterLazySingleton<IAvatarCache>(() =>
            {
                var options = Options.Create(new MemoryCacheOptions
                {
                    SizeLimit = 128 // maximum 128 cached bitmaps
                });
                return new AvatarCache(new MemoryCache(options));
            });
            
            services.RegisterLazySingleton<IAvatarLoader>(() =>
            {
                var platform = services.GetService<IPlatform>();
                var storage = services.GetService<IStorage>();
                var fileLoader = services.GetService<IFileLoader>();
                var avatarCache = services.GetService<IAvatarCache>();
                var colorMapper = services.GetService<IColorMapper>();
                
                return new AvatarLoader(
                    platform,
                    storage,
                    fileLoader,
                    avatarCache,
                    colorMapper);
            });
            
            // previews
            services.RegisterLazySingleton<IPreviewCache>(() =>
            {
                var options = Options.Create(new MemoryCacheOptions
                {
                    SizeLimit = 16 // maximum 16 cached bitmaps
                });
                return new PreviewCache(new MemoryCache(options));
            });
            
            services.RegisterLazySingleton<IPreviewLoader>(() =>
            {
                var fileLoader = services.GetService<IFileLoader>();
                var previewCache = services.GetService<IPreviewCache>();
                
                return new PreviewLoader(
                    fileLoader,
                    previewCache);
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
            // messenger
            services.RegisterLazySingleton<IBasicMessageModelFactory>(() =>
            {
                return new BasicMessageModelFactory();
            });
            
            services.RegisterLazySingleton<INoteMessageModelFactory>(() =>
            {
                return new NoteMessageModelFactory();
            });
            
            services.RegisterLazySingleton<ISpecialMessageModelFactory>(() =>
            {
                var stringFormatter = new StringFormatter();
                return new SpecialMessageModelFactory(stringFormatter);
            });
            
            services.RegisterLazySingleton<IVisualMessageModelFactory>(() =>
            {
                return new VisualMessageModelFactory();
            });
            
            services.RegisterLazySingleton<IMessageModelFactory>(() =>
            {
                var basicMessageModelFactory = services.GetService<IBasicMessageModelFactory>();
                var noteMessageModelFactory = services.GetService<INoteMessageModelFactory>();
                var specialMessageModelFactory = services.GetService<ISpecialMessageModelFactory>();
                var visualMessageModelFactory = services.GetService<IVisualMessageModelFactory>();
                
                var stringFormatter = new StringFormatter();
                
                return new MessageModelFactory(
                    basicMessageModelFactory,
                    noteMessageModelFactory,
                    specialMessageModelFactory,
                    visualMessageModelFactory,
                    stringFormatter);
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