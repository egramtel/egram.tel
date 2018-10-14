using System;
using System.Reactive;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IExplorerTrigger : IObservable<ExplorerSignal>
    {
        IDisposable Trigger(ExplorerSignal signal);
    }
}