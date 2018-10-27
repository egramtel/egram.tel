using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using ReactiveUI;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Settings;
using Tel.Egram.Components.Workspace.Navigation;
using Tel.Egram.Gui.Views.Messenger;
using Tel.Egram.Gui.Views.Settings;
using Tel.Egram.Gui.Views.Workspace;
using Tel.Egram.Gui.Views.Workspace.Navigation;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Workspace
{
    public class WorkspaceController : BaseController<WorkspaceControlModel>
    {
        private readonly IActivator<NavigationControlModel> _navigationActivator;
        private IController<NavigationControlModel> _navigationController;
        
        private readonly IActivator<Section, MessengerControlModel> _messengerActivator;
        private IController<MessengerControlModel> _messengerController;
        
        private readonly IActivator<SettingsControlModel> _settingsActivator;
        private IController<SettingsControlModel> _settingsController;

        public WorkspaceController(
            IActivator<NavigationControlModel> navigationActivator,
            IActivator<Section, MessengerControlModel> messengerActivator,
            IActivator<SettingsControlModel> settingsActivator)
        {
            _navigationActivator = navigationActivator;
            _messengerActivator = messengerActivator;
            _settingsActivator = settingsActivator;

            BindNavigation().DisposeWith(this);
        }

        private IDisposable BindNavigation()
        {
            var model = _navigationActivator.Activate(ref _navigationController);
            Model.NavigationModel = model;
            
            return model.WhenAnyValue(m => m.SelectedTabIndex)
                .Select(index => (ContentKind)index)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
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
            var messengerModel = _messengerActivator.Deactivate(ref _messengerController);
            Model.MessengerModel = messengerModel;

            _settingsActivator.Deactivate(ref _settingsController);
            var settingsModel = _settingsActivator.Activate(ref _settingsController);
            Model.SettingsModel = settingsModel;
        }

        private void InitMessenger(ContentKind kind)
        {
            var section = (Section) kind;
            
            var settingsModel = _settingsActivator.Deactivate(ref _settingsController);
            Model.SettingsModel = settingsModel;
            
            _messengerActivator.Deactivate(ref _messengerController);
            var messengerModel = _messengerActivator.Activate(section, ref _messengerController);
            Model.MessengerModel = messengerModel;
        }

        public override void Dispose()
        {
            _navigationActivator.Deactivate(ref _navigationController);
            _messengerActivator.Deactivate(ref _messengerController);
            _settingsActivator.Deactivate(ref _settingsController);
            
            base.Dispose();
        }
    }
}