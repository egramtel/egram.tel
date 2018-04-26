using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class Segment : ExplorerEntity
    {
        public Segment(string name, ExplorerEntityKind kind)
            : base(ExplorerEntityKind.Header | kind)
        {
            Name = name;
        }
        
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }
}