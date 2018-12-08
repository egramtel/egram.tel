using System;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Messaging.Messages
{
    public interface IMessageLoader
    {
        IObservable<Message> LoadMessages(Aggregate aggregate, AggregateLoadingState state);
        
        IObservable<Message> LoadNextMessages(Chat chat, long fromMessageId, int limit);
        IObservable<Message> LoadPrevMessages(Chat chat, long fromMessageId, int limit);

        IObservable<Message> LoadPinnedMessage(Chat chat);
    }
}