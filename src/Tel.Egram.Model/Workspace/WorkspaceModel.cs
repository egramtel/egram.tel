using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Model.Messenger;
using Tel.Egram.Model.Settings;
using Tel.Egram.Model.Workspace.Navigation;

namespace Tel.Egram.Model.Workspace
{
    [AddINotifyPropertyChangedInterface]
    public class WorkspaceModel : ISupportsActivation
    {
        public NavigationModel NavigationModel { get; set; }
        
        public MessengerModel MessengerModel { get; set; }
        
        public SettingsModel SettingsModel { get; set; }
        
        public int ContentIndex { get; set; }

        public WorkspaceModel()
        {
            this.WhenActivated(disposables =>
            {
                this.BindNavigation()
                    .DisposeWith(disposables);
            });
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}