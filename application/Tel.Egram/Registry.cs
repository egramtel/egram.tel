using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TdLib;
using Tel.Egram.Authentication;
using Tel.Egram.Components;
using Tel.Egram.Components.Application;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Messenger.Editor;
using Tel.Egram.Components.Messenger.Explorer;
using Tel.Egram.Components.Messenger.Informer;
using Tel.Egram.Components.Settings;
using Tel.Egram.Components.Settings.Connection;
using Tel.Egram.Components.Workspace;
using Tel.Egram.Components.Workspace.Navigation;
using Tel.Egram.Graphics;
using Tel.Egram.Gui;
using Tel.Egram.Gui.Views.Application;
using Tel.Egram.Gui.Views.Authentication;
using Tel.Egram.Gui.Views.Messenger;
using Tel.Egram.Gui.Views.Messenger.Catalog;
using Tel.Egram.Gui.Views.Messenger.Editor;
using Tel.Egram.Gui.Views.Messenger.Explorer;
using Tel.Egram.Gui.Views.Messenger.Informer;
using Tel.Egram.Gui.Views.Workspace;
using Tel.Egram.Gui.Views.Workspace.Navigation;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Messaging.Users;
using Tel.Egram.Models.Application;
using Tel.Egram.Models.Authentication;
using Tel.Egram.Models.Messenger;
using Tel.Egram.Models.Messenger.Catalog;
using Tel.Egram.Models.Messenger.Editor;
using Tel.Egram.Models.Messenger.Explorer;
using Tel.Egram.Models.Messenger.Informer;
using Tel.Egram.Models.Workspace;
using Tel.Egram.Models.Workspace.Navigation;
using Tel.Egram.Persistance;
using Tel.Egram.Settings;
using Tel.Egram.Utils;
using Tel.Egram.Utils.TdLib;

namespace Tel.Egram
{
    public static class Registry
    {
        public static void AddUtils(this IServiceCollection services)
        {
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
            services.AddScoped<IMessageSender, MessageSender>();
            
            // users
            services.AddScoped<IUserLoader, UserLoader>();
            
            // auth
            services.AddScoped<IAuthenticator, Authenticator>();
        }

        public static void AddComponents(this IServiceCollection services)
        {
            services.AddScoped(typeof(IActivator<>), typeof(Activator<>));
            services.AddScoped(typeof(IActivator<,>), typeof(Activator<,>));
        }
        
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IController<MainWindowModel>, ApplicationController>();
            
            services.AddScoped<MainApplication>();
            
            services.AddScoped(provider => new MainApplication.Initializer(() =>
            {
                var db = provider.GetService<IDatabaseContextFactory>().CreateDbContext();
                db.Database.Migrate();
                
                var hub = provider.GetService<Hub>();
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
                
                return Disposable.Create(() =>
                {
                    hub.Stop();
                });
            }));
        }
        
        public static void AddAuthentication(this IServiceCollection services)
        {
            services.AddTransient<IController<AuthenticationModel>, AuthenticationController>();
        }
        
        public static void AddMessenger(this IServiceCollection services)
        {
            services.AddTransient<IController<MessengerModel>, MessengerController>();
            services.AddTransient<IController<CatalogModel>, CatalogController>();
            services.AddTransient<IController<EditorModel>, EditorController>();
            services.AddTransient<IController<ExplorerModel>, ExplorerController>();
            services.AddTransient<IController<InformerModel>, InformerController>();
            
            services.AddTransient<IAvatarManager, AvatarManager>();
            services.AddTransient<IMessageManager, MessageManager>();
            services.AddTransient<IMessageModelFactory, MessageModelFactory>();
            
            services.AddScoped<ICatalogProvider, CatalogProvider>();
        }
        
        public static void AddPopup(this IServiceCollection services)
        {
            services.AddScoped<IApplicationPopupController, ApplicationPopupController>();
            services.AddScoped<IPopupController>(p => p.GetService<IApplicationPopupController>());
        }
        
        public static void AddSettings(this IServiceCollection services)
        {
            services.AddTransient<ISettingsController, SettingsController>();
        }
        
        public static void AddWorkspace(this IServiceCollection services)
        {
            services.AddTransient<IController<WorkspaceModel>, WorkspaceController>();
            services.AddTransient<IController<NavigationModel>, NavigationController>();
        }

        public static void AddReflection(this IServiceCollection services)
        {
            // factories
            services.AddScoped(typeof(IFactory<>), typeof(Factory<>));
            services.AddScoped(typeof(IFactory<,>), typeof(Factory<,>));

            // type mapper
            var typeMapper = new TypeMapper(services);
            services.AddScoped<ITypeMapper>(_ => typeMapper);
        }
    }
}