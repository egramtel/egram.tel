using TdLib;

namespace Tel.Egram.Feeds
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