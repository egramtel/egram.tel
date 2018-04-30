using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class Segment : ExplorerEntity
    {   
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }
}