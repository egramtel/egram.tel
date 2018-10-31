using PropertyChanged;
using Tel.Egram.Models.Messenger;
using Tel.Egram.Models.Settings;
using Tel.Egram.Models.Workspace.Navigation;

namespace Tel.Egram.Models.Workspace
{
    [AddINotifyPropertyChangedInterface]
    public class WorkspaceModel
    {
        public NavigationModel NavigationModel { get; set; }
        
        public MessengerModel MessengerModel { get; set; }
        
        public SettingsModel SettingsModel { get; set; }
        
        public int ContentIndex { get; set; }
    }
}