using TdLib;

namespace Tel.Egram.Messaging.Chats
{
    public class Chat
    {
        public TdApi.Chat ChatData { get; set; }
        
        public TdApi.User User { get; set; }
    }
}