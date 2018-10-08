using TdLib;

namespace Tel.Egram.Messaging.Chats
{
    public class ChatFeed : Feed
    {
        public TdApi.Chat Chat { get; }
        
        public ChatFeed(TdApi.Chat chat)
        {
            Chat = chat;
        }
    }
}