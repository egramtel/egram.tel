using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils.Reactive;
using Tel.Egram.Utils.TdLib;

namespace Tel.Egram.Messaging.Messages
{
    public class MessageLoader : IMessageLoader
    {
        private readonly IAgent _agent;
        private readonly int _limit = 10;

        public MessageLoader(IAgent agent)
        {
            _agent = agent;
        }

        public IObservable<Message> LoadMessages(
            Aggregate feed,
            AggregateLoadingState state)
        {
            return LoadAggregateMessages(feed, state)
                .SelectSeq(MapToMessage);
        }

        public IObservable<Message> LoadPrevMessages(
            Chat feed,
            long fromMessageId,
            int limit)
        {
            return GetMessages(feed.ChatData, fromMessageId, limit, 0)
                .SelectSeq(MapToMessage);
        }

        public IObservable<Message> LoadNextMessages(
            Chat feed,
            long fromMessageId,
            int limit)
        {   
            return GetMessages(feed.ChatData, fromMessageId, limit, -limit)
                .Where(m => m.Id != fromMessageId)
                .SelectSeq(MapToMessage);
        }

        public IObservable<Message> LoadPinnedMessage(Chat chat)
        {
            return GetPinnedMessage(chat.ChatData)
                .Where(m => m != null)
                .SelectSeq(MapToMessage);
        }

        private IObservable<Message> MapToMessage(TdApi.Message msg)
        {
            return _agent.Execute(new TdApi.GetChat
                {
                    ChatId = msg.ChatId
                })
                .Select(chat => new Message
                {
                    MessageData = msg,
                    Chat = chat
                })
                .SelectSeq(message =>
                {
                    if (message.MessageData.SenderUserId != 0)
                    {
                        return _agent.Execute(new TdApi.GetUser
                            {
                                UserId = message.MessageData.SenderUserId
                            })
                            .Select(user => new Message
                            {
                                MessageData = message.MessageData,
                                Chat = message.Chat,
                                User = user
                            });
                    }

                    return Observable.Return(message);
                });
        }

        private IObservable<TdApi.Message> LoadAggregateMessages(
            Aggregate aggregate,
            AggregateLoadingState state)
        {
            var actualLimit = _limit;
            
            var list = aggregate.Chats.Select(f =>
            {
                var stackedCount = state.CountStackedMessages(f.ChatData.Id);
                
                return Enumerable.Range(0, stackedCount)
                    .Select(_ => state.PopMessageFromStack(f.ChatData.Id)) // get stacked messages for this chat
                    .ToObservable()
                    .Concat(stackedCount < _limit
                        ? LoadChannelMessages(f, new ChatLoadingState // load messages from the server
                            {
                                LastMessageId = state.GetLastMessageId(f.ChatData.Id)
                            }, _limit, 0)
                        : Observable.Empty<TdApi.Message>())
                    .CollectToList()
                    .Do(l =>
                    {
                        // api has no guarantees about actual number of messages returned
                        // actual limit would be equal to min number of messages for all feeds
                        // unless it is zero
                        if (l.Count > 0 && l.Count < actualLimit)
                        {
                            actualLimit = l.Count;
                        }
                    })
                    .SelectMany(messages => messages);
            });
            
            return list.Merge()
                .CollectToList()
                .SelectMany(l =>
                {
                    // make sure all messages are unique
                    var all = l.GroupBy(m => m.Id)
                        .Select(g => g.First())
                        .OrderByDescending(m => m.Date)
                        .ToArray();

                    var toBeReturned = all.Take(actualLimit);
                    var toBeStacked = all.Skip(actualLimit);

                    // remember last message id
                    foreach (var message in all.Reverse())
                    {
                        state.SetLastMessageId(message.ChatId, message.Id);
                    }
                    
                    // put into stack
                    foreach (var message in toBeStacked.Reverse())
                    {
                        state.PushMessageToStack(message.ChatId, message);
                    }

                    return toBeReturned;
                });
        }

        private IObservable<TdApi.Message> LoadChannelMessages(
            Chat chat,
            ChatLoadingState state,
            int limit,
            int offset)
        {
            // get messages for corresponding chat
            return GetMessages(chat.ChatData, state.LastMessageId, limit, offset)
                .Do(message =>
                {
                    if (state.LastMessageId < message.Id)
                    {
                        state.LastMessageId = message.Id;
                    }
                });
        }

        private IObservable<TdApi.Message> GetMessages(
            TdApi.Chat chat,
            long fromMessageId,
            int limit,
            int offset)
        {   
            return _agent.Execute(new TdApi.GetChatHistory
                {
                    ChatId = chat.Id,
                    FromMessageId = fromMessageId,
                    Limit = limit,
                    Offset = offset >= 0 ? 0 : offset + 2, // limit must be greater than -offset by 2
                    OnlyLocal = false
                })
                .SelectMany(history => history.Messages_);
        }

        private IObservable<TdApi.Message> GetPinnedMessage(
            TdApi.Chat chat)
        {
            return _agent.Execute(new TdApi.GetChatPinnedMessage
            {
                ChatId = chat.Id
            });
        }
    }
}