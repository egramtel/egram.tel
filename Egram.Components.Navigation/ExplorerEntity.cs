using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class ExplorerEntity : ReactiveObject
    {
        public ExplorerEntityKind Kind { get; set; }

        public bool IsHeader => Kind.HasFlag(ExplorerEntityKind.Header);

        public bool HasLabel => !IsHeader;

        public bool HasAvatar => !IsHeader;
        
        public bool HasIndicator => false;
    }
}