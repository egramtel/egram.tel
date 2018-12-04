using TdLib;

namespace Tel.Egram.Messaging.Notifications
{
    public class Notification
    {
        public TdApi.Chat Chat { get; set; }
        
        public TdApi.Message Message { get; set; }
    }
}