using Tel.Egram.Gui.Views.Messenger;
using Tel.Egram.Gui.Views.Settings;
using Tel.Egram.Gui.Views.Workspace.Navigation;

namespace Tel.Egram.Gui.Views.Workspace
{
    public class WorkspaceControlModel
    {
        public NavigationControlModel NavigationModel { get; set; }
        
        public MessengerControlModel MessengerModel { get; set; }
        
        public SettingsControlModel SettingsModel { get; set; }
        
        public int ContentIndex { get; set; }
    }
}