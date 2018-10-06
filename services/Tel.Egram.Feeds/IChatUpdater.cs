using System;
using System.Reactive;

namespace Tel.Egram.Feeds
{
    public interface IChatUpdater
    {
        IObservable<Unit> GetOrderUpdates();

        IObservable<Chat> GetChatUpdates();
    }
}