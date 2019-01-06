using System;
using System.Reactive;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Services.Utils.TdLib;

namespace Tel.Egram.Services.Messaging.Chats
{
    public class ChatUpdater : IChatUpdater
    {
        private readonly IAgent _agent;

        public ChatUpdater(IAgent agent)
        {
            _agent = agent;
        }

        public IObservable<Unit> GetOrderUpdates()
        {
            var newUpdates = _agent.Updates.OfType<TdApi.Update.UpdateNewChat>()
                .Select(_ => Unit.Default);
            var orderUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatOrder>()
                .Select(_ => Unit.Default);
            var messageUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatLastMessage>()
                .Select(_ => Unit.Default);
            var pinnedUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatIsPinned>()
                .Select(_ => Unit.Default);
            var draftUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatDraftMessage>()
                .Select(_ => Unit.Default);

            return newUpdates
                .Merge(orderUpdates)
                .Merge(messageUpdates)
                .Merge(pinnedUpdates)
                .Merge(draftUpdates);
        }

        public IObservable<Chat> GetChatUpdates()
        {
            var titleUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatTitle>()
                .SelectMany(u => GetChat(u.ChatId));
            var photoUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatPhoto>()
                .SelectMany(u => GetChat(u.ChatId));
            var inboxUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatReadInbox>()
                .SelectMany(u => GetChat(u.ChatId));
            var messageUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatLastMessage>()
                .SelectMany(u => GetChat(u.ChatId));
                
            return titleUpdates
                .Merge(photoUpdates)
                .Merge(inboxUpdates)
                .Merge(messageUpdates);
        }

        private IObservable<Chat> GetChat(long chatId)
        {
            return _agent.Execute(new TdApi.GetChat
                {
                    ChatId = chatId
                })
                .SelectMany(chat =>
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
                });
        }

        private IObservable<TdApi.User> GetUser(int id)
        {
            return _agent.Execute(new TdApi.GetUser
            {
                UserId = id
            });
        }
    }
}