using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Messenger
{
    public class AggregateMessengerContext : MessengerContext
    {
        private readonly IAggregateService _aggregateService;
        private readonly Aggregate _aggregate;

        public AggregateMessengerContext(
            IAggregateService aggregateService,
            Aggregate aggregate
            )
        {
            _aggregateService = aggregateService;
            _aggregate = aggregate;

            IsMessageEditorVisible = false;
        }
    }
}