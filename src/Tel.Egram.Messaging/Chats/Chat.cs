using TdLib;

namespace Tel.Egram.Messaging.Chats
{
    public class Chat : Target
    {
        public TdApi.Chat ChatData { get; set; }
        
        public TdApi.User User { get; set; }
    }
}