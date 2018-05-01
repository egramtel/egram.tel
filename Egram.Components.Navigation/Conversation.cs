using Avalonia.Media.Imaging;
using Egram.Components.Navigation;
using ReactiveUI;

namespace Egram.Components.Navigation
{
    public class Conversation : ExplorerEntity
    {
        private TD.Chat _chat;
        public TD.Chat Chat
        {
            get => _chat;
            set => this.RaiseAndSetIfChanged(ref _chat, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

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