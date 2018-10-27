using PropertyChanged;
using Tel.Egram.Gui.Views.Authentication;
using Tel.Egram.Gui.Views.Workspace;

namespace Tel.Egram.Gui.Views.Application
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowModel
    {
        public StartupPageModel StartupPageModel { get; set; }
        
        public AuthenticationPageModel AuthenticationPageModel { get; set; }
        
        public WorkspacePageModel WorkspacePageModel { get; set; }
        
        public PopupControlModel PopupControlModel { get; set; }
        
        public int PageIndex { get; set; }
        
        public string WindowTitle { get; set; }
    }
    
    public enum PopupKind
    {
        Hidden = 0,
        Proxy = 1
    }
}