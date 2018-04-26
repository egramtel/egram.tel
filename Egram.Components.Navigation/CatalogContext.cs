using System;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class CatalogContext : ReactiveObject, IDisposable
    {
        public CatalogContext(Segment segment)
        {
            
        }

        public void Dispose()
        {
            
        }
    }

    public class CatalogContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CatalogContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CatalogContext FromSegment(Segment segment)
        {
            return new CatalogContext(segment);
        }
    }
}