using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Services.Utils.Reactive;
using Tel.Egram.Services.Utils.TdLib;

namespace Tel.Egram.Services.Messaging.Chats
{
    public class FeedLoader : IFeedLoader
    {
        private readonly IAgent _agent;

        public FeedLoader(IAgent agent)
        {
            _agent = agent;
        }
        
        public IObservable<Aggregate> LoadAggregate()
        {
            return GetAllChats(new List<TdApi.Chat>())
                .Where(chat =>
                {
                    var type = chat.Type as TdApi.ChatType.ChatTypeSupergroup;
                    return !(type is null) && type.IsChannel;
                })
                .Select(chat => new Chat
                {
                    ChatData = chat
                })
                .CollectToList()
                .Select(list => new Aggregate(list));
        }

        public IObservable<Chat> LoadChat(long chatId)
        {
            return _agent.Execute(new TdApi.GetChat
                {
                    ChatId = chatId
                })
                .Select(chat => new Chat
                {
                    ChatData = chat
                });
        }

        private IObservable<TdApi.Chat> GetAllChats(
            List<TdApi.Chat> chats,
            long offsetOrder = long.MaxValue,
            long offsetChatId = 0)
        {
            int limit = 100;
            
            return GetChats(offsetOrder, offsetChatId, limit)
                .CollectToList()
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