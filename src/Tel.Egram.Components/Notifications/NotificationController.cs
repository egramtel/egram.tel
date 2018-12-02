using System.Reactive.Subjects;

namespace Tel.Egram.Components.Notifications
{
    public class NotificationController : INotificationController
    {
        public Subject<NotificationModel> Trigger { get; } = new Subject<NotificationModel>();
        
        public void Show(NotificationModel model)
        {
            Trigger.OnNext(model);
        }
    }
}