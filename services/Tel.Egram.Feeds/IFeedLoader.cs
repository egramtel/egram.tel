using System;

namespace Tel.Egram.Feeds
{
    public interface IFeedLoader
    {
        IObservable<AggregateFeed> LoadAggregate();

        IObservable<ChatFeed> LoadChat(long chatId);
    }
}