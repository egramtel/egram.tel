using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Messenger
{
    public class AggregateMessengerContext : MessengerContext
    {
        private readonly Aggregate _aggregate;

        public AggregateMessengerContext(Aggregate aggregate)
        {
            _aggregate = aggregate;

            IsMessageEditorVisible = false;
        }
    }
}