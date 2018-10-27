using PropertyChanged;
using Tel.Egram.Graphics;

namespace Tel.Egram.Gui.Views.Workspace.Navigation
{
    [AddINotifyPropertyChangedInterface]
    public class NavigationControlModel
    {
        public Avatar Avatar { get; set; }

        public int SelectedTabIndex { get; set; } = 2;
    }
}