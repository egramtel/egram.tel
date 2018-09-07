using ReactiveUI;
using Tel.Egram.Components.Catalog;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Content.Direct
{
    public class DirectContentContext : ChatContentContext
    {
        public DirectContentContext(
            IFactory<CatalogKind, CatalogContext> catalogContextFactory
            )
            : base(ContentKind.Direct)
        {
            
        }

        public override void Dispose()
        {
            
        }
    }
}