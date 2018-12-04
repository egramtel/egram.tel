using System;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using Tel.Egram.Components.Notifications;
using Tel.Egram.Messaging.Notifications;

namespace Tel.Egram.Components.Messenger
{
    public static class NotificationLogic
    {
        public static IDisposable BindNotifications(
            this MessengerModel model)
        {
            return BindNotifications(
                model,
                Locator.Current.GetService<INotificationSource>(),
                Locator.Current.GetService<INotificationController>());
        }
        
        public static IDisposable BindNotifications(
            this MessengerModel model,
            INotificationSource notificationSource,
            INotificationController notificationController)
        {
            var chats = notificationSource.ChatNotifications();
            var messages = notificationSource.MessagesNotifications();

            return chats.Merge(messages)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(notification =>
                {
                    notificationController.Show(new NotificationModel
                    {
                        
                    });
                });
        }
    }
}