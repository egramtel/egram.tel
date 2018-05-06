using Egram.Components.I18N;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class Segment : ExplorerEntity
    {   
        private Phrase _name;
        public Phrase Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }
    }
}