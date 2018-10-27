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
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Workspace
{
    public class WorkspaceController
        : BaseController<WorkspacePageModel>, IWorkspaceController
    {
        private readonly IFactory<INavigationController> _navigationControllerFactory;
        private INavigationController _navigationController;
        
        private readonly IFactory<Section, IMessengerController> _messengerControllerFactory;
        private IMessengerController _messengerController;
        
        private readonly IFactory<ISettingsController> _settingControllerFactory;
        private ISettingsController _settingsController;

        public WorkspaceController(
            IFactory<INavigationController> navigationControllerFactory,
            IFactory<Section, IMessengerController> messengerControllerFactory,
            IFactory<ISettingsController> settingControllerFactory)
        {
            _navigationControllerFactory = navigationControllerFactory;
            _messengerControllerFactory = messengerControllerFactory;
            _settingControllerFactory = settingControllerFactory;

            BindNavigation()
                .DisposeWith(this);
        }

        private IDisposable BindNavigation()
        {
            _navigationController = _navigationControllerFactory.Create();
            Model.NavigationControlModel = _navigationController.Model;
            
            return Model.NavigationControlModel.WhenAnyValue(m => m.SelectedTabIndex)
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
            if (Model.SettingsControlModel == null)
            {   
                _messengerController?.Dispose();
                _messengerController = null;
                Model.MessengerControlModel = null;
                
                _settingsController = _settingControllerFactory.Create();
                Model.SettingsControlModel = _settingsController.Model;
            }
        }

        private void InitMessenger(ContentKind kind)
        {
            if (Model.MessengerControlModel == null)
            {
                _settingsController?.Dispose();
                _settingsController = null;
                Model.SettingsControlModel = null;
                
                var section = (Section) kind;
                _messengerController = _messengerControllerFactory.Create(section);
                Model.MessengerControlModel = _messengerController.Model;
            }
        }

        public override void Dispose()
        {
            _navigationController?.Dispose();
            _messengerController?.Dispose();
            _settingsController?.Dispose();
            
            base.Dispose();
        }
    }
}