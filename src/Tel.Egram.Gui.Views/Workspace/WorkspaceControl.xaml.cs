using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Workspace;

namespace Tel.Egram.Gui.Views.Workspace
{
    public class WorkspaceControl : BaseControl<WorkspaceModel>
    {
        public WorkspaceControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
