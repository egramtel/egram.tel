using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Settings;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Workspace
{
    [AddINotifyPropertyChangedInterface]
    public class WorkspaceModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();
        
        public int ContentIndex { get; set; }
        
        public NavigationModel NavigationModel { get; set; }
        
        public MessengerModel MessengerModel { get; set; }
        
        public SettingsModel SettingsModel { get; set; }
        
        public WorkspaceModel(
            IFactory<NavigationModel> navigationModelFactory,
            IFactory<CatalogKind, MessengerModel> messengerModelFactory,
            IFactory<SettingsModel> settingsModelFactory
            )
        {
            BindNavigation(navigationModelFactory, messengerModelFactory, settingsModelFactory)
                .DisposeWith(_modelDisposable);
        }

        private IDisposable BindNavigation(
            IFactory<NavigationModel> navigationModelFactory,
            IFactory<CatalogKind, MessengerModel> messengerModelFactory,
            IFactory<SettingsModel> settingsModelFactory
            )
        {
            NavigationModel = navigationModelFactory.Create();
            
            return NavigationModel.WhenAnyValue(context => context.SelectedTabIndex)
                .Select(index => (ContentKind)index)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(kind =>
                {
                    switch (kind)
                    {
                        case ContentKind.Settings:
                            ContentIndex = 1;
                            InitSettings(settingsModelFactory);
                            break;
                        
                        default:
                            ContentIndex = 0;
                            InitMessenger(messengerModelFactory, kind);
                            break;
                    }
                });
        }

        private void InitMessenger(
            IFactory<CatalogKind, MessengerModel> messengerModelFactory,
            ContentKind contentKind
            )
        {
            SettingsModel?.Dispose();
            
            var catalogKind = (CatalogKind) contentKind;
            MessengerModel?.Dispose();
            MessengerModel = messengerModelFactory.Create(catalogKind);
        }

        private void InitSettings(IFactory<SettingsModel> settingsModelFactory)
        {
            MessengerModel?.Dispose();

            SettingsModel = settingsModelFactory.Create();
        }
        
        public void Dispose()
        {
            NavigationModel?.Dispose();
            MessengerModel?.Dispose();
            SettingsModel?.Dispose();
            
            _modelDisposable.Dispose();
        }
    }
}