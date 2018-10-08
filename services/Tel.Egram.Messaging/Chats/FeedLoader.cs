using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Utils.TdLib;

namespace Tel.Egram.Messaging.Chats
{
    public class FeedLoader : IFeedLoader
    {
        private readonly IAgent _agent;

        public FeedLoader(IAgent agent)
        {
            _agent = agent;
        }
        
        public IObservable<AggregateFeed> LoadAggregate()
        {
            return GetAllChats(new List<TdApi.Chat>())
                .Where(chat =>
                {
                    var type = chat.Type as TdApi.ChatType.ChatTypeSupergroup;
                    return !(type is null) && type.IsChannel;
                })
                .Select(chat => new ChatFeed(chat))
                .Aggregate(new List<ChatFeed>(), (list, feed) =>
                {
                    list.Add(feed);
                    return list;
                })
                .Select(list => new AggregateFeed(list));
        }

        public IObservable<ChatFeed> LoadChat(long chatId)
        {
            return _agent.Execute(new TdApi.GetChat
                {
                    ChatId = chatId
                })
                .Select(chat => new ChatFeed(chat));
        }

        private IObservable<TdApi.Chat> GetAllChats(
            List<TdApi.Chat> chats,
            long offsetOrder = long.MaxValue,
            long offsetChatId = 0)
        {
            int limit = 100;
            
            return GetChats(offsetOrder, offsetChatId, limit)
                .Aggregate(new List<TdApi.Chat>(), (list, chat) =>
                {
                    list.Add(chat);
                    return list;
                })
                .SelectMany(list =>
                {
                    if (list.Count > 0)
                    {
                        var lastChat = list.Last();
                        chats.AddRange(list);
                        return GetAllChats(chats, lastChat.Order, lastChat.Id);
                    }
                    
                    return chats.ToObservable();
                });
        }

        private IObservable<TdApi.Chat> GetChats(long offsetOrder, long offsetChatId, int limit)
        {
            return _agent.Execute(new TdApi.GetChats
                {
                    OffsetOrder = offsetOrder,
                    OffsetChatId = offsetChatId,
                    Limit = limit
                })
                .SelectMany(result => result.ChatIds)
                .SelectMany(chatId => _agent.Execute(new TdApi.GetChat
                {
                    ChatId = chatId
                }));
        }
    }
}