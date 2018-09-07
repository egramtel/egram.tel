using System;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Tel.Egram.Components.Catalog;
using Tel.Egram.Components.Content.Direct;
using Tel.Egram.Components.Content.Home;
using Tel.Egram.Components.Messenger;

namespace Tel.Egram.Components.Content
{
    public abstract class ContentContext : ReactiveObject, IDisposable
    {
        public bool IsHomeContentVisible { get; }
        
        public bool IsDirectContentVisible { get; }
        
        public bool IsGroupsContentVisible { get; }
        
        public bool IsBotsContentVisible { get; }
        
        public ContentContext(ContentKind kind)
        {
            switch (kind)
            {
                case ContentKind.Home:
                    IsHomeContentVisible = true;
                    break;
                case ContentKind.Direct:
                    IsDirectContentVisible = true;
                    break;
                case ContentKind.Groups:
                    IsGroupsContentVisible = true;
                    break;
                case ContentKind.Bots:
                    IsBotsContentVisible = true;
                    break;
            }
        }

        public abstract void Dispose();
    }
}