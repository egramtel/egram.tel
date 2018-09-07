using System;
using Tel.Egram.Feeds;

namespace Tel.Egram.Messages
{
    public interface IMessageLoader
    {
        IObservable<Message> LoadMessages(AggregateFeed aggregateFeed, AggregateLoading state);
        
        IObservable<Message> LoadMessages(ChatFeed chatFeed, ChatLoading state);
    }
}