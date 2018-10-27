using Tel.Egram.Gui.Views.Messenger;
using Tel.Egram.Gui.Views.Settings;

namespace Tel.Egram.Gui.Views.Workspace
{
    public class WorkspacePageModel
    {
        public NavigationControlModel NavigationControlModel { get; set; }
        
        public MessengerControlModel MessengerControlModel { get; set; }
        
        public SettingsControlModel SettingsControlModel { get; set; }
        
        public int ContentIndex { get; set; }
    }
}