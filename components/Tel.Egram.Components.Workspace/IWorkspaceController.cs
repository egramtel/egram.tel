using System;
using Tel.Egram.Gui.Views.Workspace;

namespace Tel.Egram.Components.Workspace
{
    public interface IWorkspaceController : IDisposable
    {
        WorkspacePageModel Model { get; }
    }
}