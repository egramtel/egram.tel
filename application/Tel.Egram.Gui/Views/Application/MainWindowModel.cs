using PropertyChanged;
using Tel.Egram.Gui.Views.Application.Popup;
using Tel.Egram.Gui.Views.Application.Startup;
using Tel.Egram.Gui.Views.Authentication;
using Tel.Egram.Gui.Views.Workspace;

namespace Tel.Egram.Gui.Views.Application
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowModel
    {
        public StartupControlModel StartupModel { get; set; }
        
        public AuthenticationControlModel AuthenticationModel { get; set; }
        
        public WorkspaceControlModel WorkspaceModel { get; set; }
        
        public PopupControlModel PopupModel { get; set; }
        
        public int PageIndex { get; set; }
        
        public string WindowTitle { get; set; }
    }
}