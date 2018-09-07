using Tel.Egram.Components.Catalog;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Content.Groups
{
    public class GroupsContentContext : ChatContentContext
    {   
        public GroupsContentContext(
            IFactory<CatalogKind, CatalogContext> catalogContextFactory
            )
            : base(ContentKind.Groups)
        {
            
        }

        public override void Dispose()
        {
            
        }
    }
}