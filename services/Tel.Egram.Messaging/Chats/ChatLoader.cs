using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Utils.TdLib;

namespace Tel.Egram.Messaging.Chats
{
    public class ChatLoader : IChatLoader
    {
        private readonly IAgent _agent;

        public ChatLoader(IAgent agent)
        {
            _agent = agent;
        }

        public IObservable<Chat> LoadChats()
        {
            return GetAllChats(new List<TdApi.Chat>())
                .Select(chat =>
                {
                    if (chat.Type is TdApi.ChatType.ChatTypePrivate type)
                    {
                        return GetUser(type.UserId)
                            .Select(user => new Chat
                            {
                                ChatData = chat,
                                User = user
                            });
                    }
                    
                    return Observable.Return(new Chat
                    {
                        ChatData = chat
                    });
                })
                .Concat();
        }

        public IObservable<Chat> LoadChannels()
        {
            return LoadChats().Where(chat =>
            {
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypeSupergroup supergroupType)
                {
                    return supergroupType.IsChannel;
                }
                return false;
            });
        }

        public IObservable<Chat> LoadDirects()
        {
            return LoadChats().Where(chat =>
            {
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypePrivate)
                {
                    return chat.User != null &&
                           chat.User.Type is TdApi.UserType.UserTypeRegular;
                }
                return false;
            });
        }

        public IObservable<Chat> LoadGroups()
        {
            return LoadChats().Where(chat =>
            {
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypeSupergroup supergroupType)
                {
                    return !supergroupType.IsChannel;
                }

                return chat.ChatData.Type is TdApi.ChatType.ChatTypeBasicGroup;
            });
        }

        public IObservable<Chat> LoadBots()
        {
            return LoadChats().Where(chat =>
            {
                if (chat.ChatData.Type is TdApi.ChatType.ChatTypePrivate)
                {
                    return chat.User != null &&
                           chat.User.Type is TdApi.UserType.UserTypeBot;
                }
                return false;
            });
        }

        private IObservable<TdApi.User> GetUser(int id)
        {
            return _agent.Execute(new TdApi.GetUser
            {
                UserId = id
            });
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
                .Select(list =>
                {
                    if (list.Count > 0)
                    {
                        var lastChat = list.Last();
                        chats.AddRange(list);
                        return GetAllChats(chats, lastChat.Order, lastChat.Id);
                    }
                    
                    return chats.ToObservable();
                })
                .Concat();
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
                .Select(chatId => _agent.Execute(new TdApi.GetChat
                {
                    ChatId = chatId
                }))
                .Concat();
        }
    }
}