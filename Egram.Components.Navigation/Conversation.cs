using Avalonia.Media.Imaging;
using Egram.Components.Navigation;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class Conversation : ExplorerEntity
    {
        private Topic _topic;
        public Topic Topic
        {
            get => _topic;
            set => this.RaiseAndSetIfChanged(ref _topic, value);
        }
        
        public string Name => Topic.Chat.Title;

        private IBitmap _avatar;
        public IBitmap Avatar
        {
            get => _avatar;
            set => this.RaiseAndSetIfChanged(ref _avatar, value);
        }

        private string _indicator;
        public string Indicator
        {
            get => _indicator;
            set => this.RaiseAndSetIfChanged(ref _indicator, value);
        }
    }
}