using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Application.Startup;
using Tel.Egram.Components.Authentication;
using Tel.Egram.Components.Popup;
using Tel.Egram.Components.Workspace;

namespace Tel.Egram.Components.Application
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