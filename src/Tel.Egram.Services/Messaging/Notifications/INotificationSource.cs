using System;

namespace Tel.Egram.Services.Messaging.Notifications
{
    public interface INotificationSource
    {
        IObservable<Notification> ChatNotifications();

        IObservable<Notification> MessagesNotifications();
    }
}