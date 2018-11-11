using System;
using System.Reactive;
using System.Reactive.Linq;
using Tel.Egram.Components.Workspace.Navigation;
using ReactiveUI;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Settings;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Workspace
{
    public static class NavigationLogic
    {
        public static IDisposable BindNavigation(
            this WorkspaceModel model)
        {
            model.NavigationModel = new NavigationModel();
            
            return model.NavigationModel.WhenAnyValue(m => m.SelectedTabIndex)
                .Select(index => (ContentKind)index)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(kind =>
                {
                    switch (kind)
                    {
                        case ContentKind.Settings:
                            InitSettings(model);
                            break;
                        
                        default:
                            InitMessenger(model, kind);
                            break;
                    }
                });
        }

        private static void InitMessenger(WorkspaceModel model, ContentKind kind)
        {
            var section = (Section) kind;
            
            model.SettingsModel = null;
            model.MessengerModel = new MessengerModel(section);
        }

        private static void InitSettings(WorkspaceModel model)
        {
            model.ContentIndex = 1;
            
            model.MessengerModel = null;
            model.SettingsModel = new SettingsModel();
        }
    }
}