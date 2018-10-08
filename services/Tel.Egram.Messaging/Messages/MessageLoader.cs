using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Messaging.Chats;
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
            AggregateFeed aggregateFeed,
            AggregateLoading state)
        {
            return LoadAggregateMessages(aggregateFeed, state)
                .SelectMany(MapToMessage);
        }

        public IObservable<Message> LoadMessages(
            ChatFeed chatFeed,
            ChatLoading state)
        {
            return LoadChannelMessages(chatFeed, state)
                .SelectMany(MapToMessage);
        }

        private IObservable<Message> MapToMessage(TdApi.Message msg)
        {
            return _agent.Execute(new TdApi.GetChat
                {
                    ChatId = msg.ChatId
                })
                .Select(chat => new Message
                {
                    Msg = msg,
                    Chat = chat
                });
        }

        private IObservable<TdApi.Message> LoadAggregateMessages(
            AggregateFeed aggregateFeed,
            AggregateLoading state)
        {
            var actualLimit = _limit;
            
            var list = aggregateFeed.Feeds.Select(f =>
            {
                var stackedCount = state.CountStackedMessages(f.Chat.Id);
                
                return Enumerable.Range(0, stackedCount)
                    .Select(_ => state.PopMessageFromStack(f.Chat.Id)) // get stacked messages for this chat
                    .ToObservable()
                    .Concat(stackedCount < _limit
                        ? LoadChannelMessages(f, new ChatLoading // load messages from the server
                            {
                                LastMessageId = state.GetLastMessageId(f.Chat.Id)
                            })
                        : Observable.Empty<TdApi.Message>())
                    .Aggregate(new List<TdApi.Message>(), (l, m) =>
                    {
                        l.Add(m);
                        return l;
                    })
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
                .Aggregate(new List<TdApi.Message>(), (l, m) =>
                {
                    l.Add(m);
                    return l;
                })
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
            ChatFeed chatFeed,
            ChatLoading state)
        {
            // get messages for corresponding chat
            return GetMessages(chatFeed.Chat, state.LastMessageId)
                .Do(message => { state.LastMessageId = message.Id; })
                .Aggregate(new List<TdApi.Message>(), (list, message) =>
                {
                    list.Add(message);
                    return list;
                })
                .SelectMany(list =>
                {
                    if (list.Count < _limit)
                    {
                        // make one more query to avoid results with small number of messages
                        var additional = GetMessages(chatFeed.Chat, state.LastMessageId)
                            .Do(message => { state.LastMessageId = message.Id; });
                        
                        return list.ToObservable().Concat(additional);
                    }

                    return list.ToObservable();
                });
        }

        private IObservable<TdApi.Message> GetMessages(TdApi.Chat chat, long fromMessageId)
        {
            return _agent.Execute(new TdApi.GetChatHistory
                {
                    ChatId = chat.Id,
                    FromMessageId = fromMessageId,
                    Limit = _limit,
                    Offset = 0,
                    OnlyLocal = false
                })
                .SelectMany(history => history.Messages_);
        }
    }
}