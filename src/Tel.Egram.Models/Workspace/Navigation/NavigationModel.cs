using PropertyChanged;
using Tel.Egram.Graphics;

namespace Tel.Egram.Models.Workspace.Navigation
{
    [AddINotifyPropertyChangedInterface]
    public class NavigationModel
    {
        public Avatar Avatar { get; set; }

        public int SelectedTabIndex { get; set; }
    }
}