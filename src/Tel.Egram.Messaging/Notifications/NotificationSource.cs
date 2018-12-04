using System;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Utils.Reactive;
using Tel.Egram.Utils.TdLib;

namespace Tel.Egram.Messaging.Notifications
{
    public class NotificationSource : INotificationSource
    {
        private readonly IAgent _agent;

        public NotificationSource(
            IAgent agent)
        {
            _agent = agent;
        }
        
        public IObservable<Notification> ChatNotifications()
        {
            return _agent.Updates
                .OfType<TdApi.Update.UpdateNewChat>()
                .Select(update => new Notification
                {
                    Chat = update.Chat
                });
        }

        public IObservable<Notification> MessagesNotifications()
        {
            return _agent.Updates
                .OfType<TdApi.Update.UpdateNewMessage>()
                .Where(u => !u.DisableNotification)
                .Select(update => update.Message)
                .SelectSeq(message =>
                {
                    return GetChat(message.ChatId)
                        .Select(chat => new Notification
                        {
                            Chat = chat,
                            Message = message
                        });
                });
        }
        
        private IObservable<TdApi.Chat> GetChat(long chatId)
        {
            return _agent.Execute(new TdApi.GetChat
                {
                    ChatId = chatId
                });
        }
    }
}