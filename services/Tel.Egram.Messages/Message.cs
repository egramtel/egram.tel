using TdLib;

namespace Tel.Egram.Messages
{
    public class Message
    {
        public TdApi.Message Msg { get; set; }
        
        public TdApi.Chat Chat { get; set; }
    }
}