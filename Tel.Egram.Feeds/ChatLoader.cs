using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.TdLib;

namespace Tel.Egram.Feeds
{
    public class ChatLoader : IChatLoader
    {
        private readonly TdAgent _agent;

        public ChatLoader(TdAgent agent)
        {
            _agent = agent;
        }

        public IObservable<Chat> LoadAllChats()
        {
            return GetAllChats(new List<TdApi.Chat>())
                .SelectMany(chat =>
                {
                    if (chat.Type is TdApi.ChatType.ChatTypePrivate type)
                    {
                        return GetUser(type.UserId)
                            .Select(user => new Chat
                            {
                                Ch = chat,
                                User = user
                            });
                    }
                    
                    return Observable.Return(new Chat
                    {
                        Ch = chat
                    });
                });
        }

        public IObservable<Chat> LoadChannels()
        {
            return LoadAllChats().Where(chat =>
            {
                if (chat.Ch.Type is TdApi.ChatType.ChatTypeSupergroup supergroupType)
                {
                    return supergroupType.IsChannel;
                }
                return false;
            });
        }

        public IObservable<Chat> LoadUsers()
        {
            return LoadAllChats().Where(chat =>
            {
                if (chat.Ch.Type is TdApi.ChatType.ChatTypePrivate)
                {
                    return chat.User != null &&
                           chat.User.Type is TdApi.UserType.UserTypeRegular;
                }
                return false;
            });
        }

        public IObservable<Chat> LoadGroups()
        {
            return LoadAllChats().Where(chat =>
            {
                if (chat.Ch.Type is TdApi.ChatType.ChatTypeSupergroup supergroupType)
                {
                    return !supergroupType.IsChannel;
                }

                return chat.Ch.Type is TdApi.ChatType.ChatTypeBasicGroup;
            });
        }

        public IObservable<Chat> LoadBots()
        {
            return LoadAllChats().Where(chat =>
            {
                if (chat.Ch.Type is TdApi.ChatType.ChatTypePrivate)
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