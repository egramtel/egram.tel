using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Workspace
{
    [DoNotNotify]
    public class WorkspacePage : UserControl
    {
        public WorkspacePage()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
