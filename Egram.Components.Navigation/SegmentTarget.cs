using Egram.Components.Navigation;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class SegmentTarget : ExplorerEntity
    {
        public readonly Topic Topic;

        public SegmentTarget(ExplorerEntityKind kind, Topic topic)
            : base(kind)
        {
            Topic = topic;
        }

        private string _indicator;
        public string Indicator
        {
            get => _indicator;
            set => this.RaiseAndSetIfChanged(ref _indicator, value);
        }

        public string Name => Topic.Chat.Title;
    }
}