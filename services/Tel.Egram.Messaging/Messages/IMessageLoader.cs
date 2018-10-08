using System;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Messaging.Messages
{
    public interface IMessageLoader
    {
        IObservable<Message> LoadMessages(AggregateFeed aggregateFeed, AggregateLoading state);
        
        IObservable<Message> LoadMessages(ChatFeed chatFeed, ChatLoading state);
    }
}