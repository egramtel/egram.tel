using TdLib;

namespace Tel.Egram.Messaging.Messages
{
    public class Message
    {
        public TdApi.Message MessageData { get; set; }
        
        public TdApi.Chat Chat { get; set; }
        
        public TdApi.User User { get; set; }
    }
}