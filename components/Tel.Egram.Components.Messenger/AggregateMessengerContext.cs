using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger
{
    public class AggregateMessengerContext : MessengerContext
    {
        public AggregateMessengerContext(
            IAggregateService aggregateService,
            Aggregate aggregate
            )
        {
            
        }
    }
}