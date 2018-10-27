using System;
using Tel.Egram.Gui.Views.Messenger.Explorer;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IExplorerController : IDisposable
    {
        ExplorerControlModel Model { get; }
    }
}