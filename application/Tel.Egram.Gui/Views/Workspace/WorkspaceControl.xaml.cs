using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Workspace
{
    [DoNotNotify]
    public class WorkspaceControl : UserControl
    {
        public WorkspaceControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
