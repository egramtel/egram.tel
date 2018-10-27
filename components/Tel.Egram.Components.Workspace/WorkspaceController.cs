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
        : BaseController, IWorkspaceController
    {
        private readonly IFactory<NavigationControlModel, INavigationController> _navigationControllerFactory;
        private INavigationController _navigationController;
        
        private readonly IFactory<MessengerControlModel, IMessengerController> _messengerControllerFactory;
        private IMessengerController _messengerController;
        
        private readonly IFactory<SettingsControlModel, ISettingsController> _settingControllerFactory;
        private ISettingsController _settingsController;

        public WorkspaceController(
            WorkspacePageModel workspacePageModel,
            IFactory<NavigationControlModel, INavigationController> navigationControllerFactory,
            IFactory<MessengerControlModel, IMessengerController> messengerControllerFactory,
            IFactory<SettingsControlModel, ISettingsController> settingControllerFactory)
        {
            _navigationControllerFactory = navigationControllerFactory;
            _messengerControllerFactory = messengerControllerFactory;
            _settingControllerFactory = settingControllerFactory;

            BindNavigation(workspacePageModel)
                .DisposeWith(this);
        }

        private IDisposable BindNavigation(WorkspacePageModel model)
        {
            var navigationModel = new NavigationControlModel();
            model.NavigationControlModel = navigationModel;
            _navigationController = _navigationControllerFactory.Create(navigationModel);
            
            return model.NavigationControlModel.WhenAnyValue(m => m.SelectedTabIndex)
                .Select(index => (ContentKind)index)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(kind =>
                {
                    switch (kind)
                    {
                        case ContentKind.Settings:
                            model.ContentIndex = 1;
                            InitSettings(model);
                            break;
                        
                        default:
                            model.ContentIndex = 0;
                            InitMessenger(model, kind);
                            break;
                    }
                });
        }

        private void InitSettings(WorkspacePageModel model)
        {
            if (model.SettingsControlModel == null)
            {
                var settingsControlModel = new SettingsControlModel();
                
                model.MessengerControlModel = null;
                model.SettingsControlModel = settingsControlModel;

                _messengerController?.Dispose();
                _messengerController = null;
                
                _settingsController = _settingControllerFactory.Create(settingsControlModel);
            }
        }

        private void InitMessenger(WorkspacePageModel model, ContentKind kind)
        {
            if (model.MessengerControlModel == null)
            {
                var section = (Section) kind;
                var messengerControlModel = MessengerControlModel.FromSection(section);
                
                model.SettingsControlModel = null;
                model.MessengerControlModel = messengerControlModel;

                _settingsController?.Dispose();
                _settingsController = null;
                
                _messengerController = _messengerControllerFactory.Create(messengerControlModel);
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