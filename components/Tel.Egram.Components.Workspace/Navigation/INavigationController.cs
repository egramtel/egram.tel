using System;
using Tel.Egram.Gui.Views.Workspace;

namespace Tel.Egram.Components.Workspace.Navigation
{
    public interface INavigationController : IDisposable
    {
        NavigationControlModel Model { get; }
    }
}