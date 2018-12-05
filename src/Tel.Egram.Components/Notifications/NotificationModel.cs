using System.Collections.Generic;
using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Messaging.Notifications;

namespace Tel.Egram.Components.Notifications
{
    [AddINotifyPropertyChangedInterface]
    public class NotificationModel : ISupportsActivation
    {
        public string Title { get; set; }
        
        public string Text { get; set; }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public static NotificationModel FromNotification(Notification notification)
        {
            var chat = notification.Chat;
            var message = notification.Message;

            if (message != null)
            {
                return new NotificationModel
                {
                    Title = "New message",
                    Text = chat.Title
                };
            }

            return new NotificationModel
            {
                Title = "New chat",
                Text = chat.Title
            };
        }

        public static NotificationModel FromNotificationList(IList<Notification> notifications)
        {
            return new NotificationModel
            {
                Title = "New messages",
                Text = "You have new messages"
            };
        }
    }
}