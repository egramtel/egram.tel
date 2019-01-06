using TdLib;

namespace Tel.Egram.Services.Messaging.Messages
{
    public class Message
    {
        public Message ReplyMessage { get; set; }
        
        public TdApi.Message MessageData { get; set; }
        
        public TdApi.Chat ChatData { get; set; }
        
        public TdApi.User UserData { get; set; }
    }
}