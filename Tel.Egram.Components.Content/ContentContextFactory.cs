using System;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Components.Content.Bots;
using Tel.Egram.Components.Content.Direct;
using Tel.Egram.Components.Content.Groups;
using Tel.Egram.Components.Content.Home;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Content
{
    public class ContentContextFactory : IFactory<ContentKind, ContentContext>
    {
        private readonly IServiceProvider _provider;

        public ContentContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public ContentContext Create(ContentKind kind)
        {
            switch (kind)
            {
                case ContentKind.Home:
                    return _provider.GetService<HomeContentContext>();
                case ContentKind.Direct:
                    return _provider.GetService<DirectContentContext>();
                case ContentKind.Groups:
                    return _provider.GetService<GroupsContentContext>();
                case ContentKind.Bots:
                    return _provider.GetService<BotsContentContext>();
            }
            
            throw new Exception("Unknown content kind");
        }
    }
}