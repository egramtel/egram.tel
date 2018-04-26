using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class ExplorerEntity : ReactiveObject
    {
        public ExplorerEntityKind Kind { get; }
        
        public ExplorerEntity(ExplorerEntityKind kind)
        {
            Kind = kind;
        }

        public bool IsHeader => Kind.HasFlag(ExplorerEntityKind.Header);
        
        public bool IsChannel => Kind == ExplorerEntityKind.Channel;
        
        public bool IsGroup => Kind == ExplorerEntityKind.Group;
        
        public bool IsBot => Kind == ExplorerEntityKind.Bot;
        
        public bool IsDirect => Kind == ExplorerEntityKind.People;

        public bool HasHeader => IsHeader;

        public bool HasLabel => !IsHeader;
        
        public bool HasIndicator => false;
    }
}