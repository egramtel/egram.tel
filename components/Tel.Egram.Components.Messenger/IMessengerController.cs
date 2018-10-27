using System;
using Tel.Egram.Gui.Views.Messenger;

namespace Tel.Egram.Components.Messenger
{
    public interface IMessengerController : IDisposable
    {
        MessengerControlModel Model { get; }
    }
}