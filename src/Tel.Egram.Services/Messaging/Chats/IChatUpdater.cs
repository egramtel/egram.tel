using System;
using System.Reactive;

namespace Tel.Egram.Services.Messaging.Chats
{
    public interface IChatUpdater
    {
        IObservable<Unit> GetOrderUpdates();

        IObservable<Chat> GetChatUpdates();
    }
}