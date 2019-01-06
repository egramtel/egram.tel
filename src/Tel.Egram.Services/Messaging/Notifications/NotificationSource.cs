using System;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Services.Utils.Reactive;
using Tel.Egram.Services.Utils.TdLib;

namespace Tel.Egram.Services.Messaging.Notifications
{
    public class NotificationSource : INotificationSource
    {
        private readonly IAgent _agent;

        public NotificationSource(
            IAgent agent)
        {
            _agent = agent;
        }
        
        /// <summary>
        /// Get notifications for new chats
        /// </summary>
        public IObservable<Notification> ChatNotifications()
        {
            return _agent.Updates
                .OfType<TdApi.Update.UpdateNewChat>()
                .Select(update => new Notification
                {
                    Chat = update.Chat
                });
        }

        /// <summary>
        /// Get notifications for new messages from chats with
        /// enabled notifications and not older than 1 minute
        /// </summary>
        public IObservable<Notification> MessagesNotifications()
        {
            return _agent.Updates
                .OfType<TdApi.Update.UpdateNewMessage>()
                .Where(u => !u.DisableNotification)
                .Where(u => u.Message.Date > DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 60)
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