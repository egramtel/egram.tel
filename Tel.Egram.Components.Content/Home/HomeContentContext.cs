using System;
using ReactiveUI;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Components.Messenger.Aggregate;
using Tel.Egram.Feeds;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Content.Home
{
    public class HomeContentContext : ChatContentContext
    {
        private readonly IFactory<AggregateFeed, AggregateMessengerContext> _aggregateMessengerFactory;

        private readonly HomeFeedInteractor _homeFeedInteractor;
        private readonly IDisposable _loadFeedSubscription;

        public HomeContentContext(
            IFactory<AggregateFeed, AggregateMessengerContext> aggregateMessengerFactory, 
            IFactory<HomeFeedInteractor> homeFeedInteractorCreator
            )
            : base(ContentKind.Home)
        {
            _aggregateMessengerFactory = aggregateMessengerFactory;

            _homeFeedInteractor = homeFeedInteractorCreator.Create();
            _loadFeedSubscription = _homeFeedInteractor.LoadFeed(this);
        }

        public void OnFeedLoaded(AggregateFeed feed)
        {
            MessengerContext = _aggregateMessengerFactory.Create(feed);
        }

        public override void Dispose()
        {
            _loadFeedSubscription.Dispose();
        }
    }
}