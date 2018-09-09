using TdLib;

namespace Tel.Egram.Feeds
{
    public class Chat : Target
    {
        public TdApi.Chat ChatData { get; set; }
        
        public TdApi.User User { get; set; }
    }
}