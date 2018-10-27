using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Workspace.Navigation
{
    [DoNotNotify]
    public class NavigationControl : UserControl
    {
        public NavigationControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
