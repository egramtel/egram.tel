using System;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Content.Home
{
    public class HomeFeedInteractor
    {
        private readonly IFeedLoader _feedLoader;
        
        public HomeFeedInteractor(
            IFeedLoader feedLoader
            )
        {
            _feedLoader = feedLoader;
        }

        public IDisposable LoadFeed(HomeContentContext context)
        {
            return _feedLoader.LoadAggregate()
                .Subscribe(feed =>
                {
                    context.OnFeedLoaded(feed);
                });
        }
    }
}