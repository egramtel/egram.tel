using System;

namespace Tel.Egram.Messaging.Notifications
{
    public interface INotificationSource
    {
        IObservable<Notification> ChatNotifications();

        IObservable<Notification> MessagesNotifications();
    }
}