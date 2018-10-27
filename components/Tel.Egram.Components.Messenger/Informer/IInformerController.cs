using System;
using Tel.Egram.Gui.Views.Messenger.Informer;

namespace Tel.Egram.Components.Messenger.Informer
{
    public interface IInformerController : IDisposable
    {
        InformerControlModel Model { get; }
    }
}