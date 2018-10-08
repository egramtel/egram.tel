using System.Collections.Generic;

namespace Tel.Egram.Messaging.Chats
{
    public class AggregateFeed : Feed
    {
        public long Id { get; }
        
        public IList<ChatFeed> Feeds { get; }

        public AggregateFeed(long id, IList<ChatFeed> feeds)
        {
            Id = id;
            Feeds = feeds;
        }

        public AggregateFeed(IList<ChatFeed> feeds)
            : this(0, feeds)
        {
        }
    }
}