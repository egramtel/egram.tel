using PropertyChanged;
using Tel.Egram.Models.Application.Popup;
using Tel.Egram.Models.Application.Startup;
using Tel.Egram.Models.Authentication;
using Tel.Egram.Models.Workspace;

namespace Tel.Egram.Models.Application
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowModel
    {
        public StartupModel StartupModel { get; set; }
        
        public AuthenticationModel AuthenticationModel { get; set; }
        
        public WorkspaceModel WorkspaceModel { get; set; }
        
        public PopupModel PopupModel { get; set; }
        
        public int PageIndex { get; set; }
        
        public string WindowTitle { get; set; }
    }
}