using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using ReactiveUI;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Settings;
using Tel.Egram.Components.Workspace.Navigation;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger;
using Tel.Egram.Models.Settings;
using Tel.Egram.Models.Workspace;
using Tel.Egram.Models.Workspace.Navigation;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Workspace
{
    public class WorkspaceController : Controller<WorkspaceModel>
    {
        private IController<NavigationModel> _navigationController;
        private IController<MessengerModel> _messengerController;
        private IController<SettingsModel> _settingsController;

        public WorkspaceController(ISchedulers schedulers)
        {
            BindNavigation(schedulers).DisposeWith(this);
        }

        private IDisposable BindNavigation(ISchedulers schedulers)
        {
            var model = Activate(ref _navigationController);
            Model.NavigationModel = model;
            
            return model.WhenAnyValue(m => m.SelectedTabIndex)
                .Select(index => (ContentKind)index)
                .SubscribeOn(schedulers.Pool)
                .ObserveOn(schedulers.Main)
                .Subscribe(kind =>
                {
                    switch (kind)
                    {
                        case ContentKind.Settings:
                            Model.ContentIndex = 1;
                            InitSettings();
                            break;
                        
                        default:
                            Model.ContentIndex = 0;
                            InitMessenger(kind);
                            break;
                    }
                });
        }

        private void InitSettings()
        {
            var messengerModel = Deactivate(ref _messengerController);
            Model.MessengerModel = messengerModel;

            var settingsModel = Activate(ref _settingsController);
            Model.SettingsModel = settingsModel;
        }

        private void InitMessenger(ContentKind kind)
        {
            var section = (Section) kind;
            
            var settingsModel = Deactivate(ref _settingsController);
            Model.SettingsModel = settingsModel;
            
            var messengerModel = Activate(section, ref _messengerController);
            Model.MessengerModel = messengerModel;
        }
    }
}