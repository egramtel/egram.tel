using System;

namespace Tel.Egram.Services.Messaging.Chats
{
    public interface IFeedLoader
    {
        IObservable<Aggregate> LoadAggregate();

        IObservable<Chat> LoadChat(long chatId);
    }
}