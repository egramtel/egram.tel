using System;

namespace Tel.Egram.Messaging.Chats
{
    public interface IFeedLoader
    {
        IObservable<AggregateFeed> LoadAggregate();

        IObservable<ChatFeed> LoadChat(long chatId);
    }
}