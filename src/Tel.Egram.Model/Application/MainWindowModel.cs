using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Model.Application.Startup;
using Tel.Egram.Model.Authentication;
using Tel.Egram.Model.Popups;
using Tel.Egram.Model.Workspace;

namespace Tel.Egram.Model.Application
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowModel : ISupportsActivation
    {
        public StartupModel StartupModel { get; set; }
        
        public AuthenticationModel AuthenticationModel { get; set; }
        
        public WorkspaceModel WorkspaceModel { get; set; }
        
        public PopupModel PopupModel { get; set; }
        
        public int PageIndex { get; set; }
        
        public string WindowTitle { get; set; }

        public string ConnectionState { get; set; }

        public MainWindowModel()
        {
            this.WhenActivated(disposables =>
            {
                this.BindAuthentication()
                    .DisposeWith(disposables);

                this.BindConnectionInfo()
                    .DisposeWith(disposables);
                
                this.BindPopup()
                    .DisposeWith(disposables);
            });
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}