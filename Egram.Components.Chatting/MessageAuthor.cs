using Avalonia.Media.Imaging;
using ReactiveUI;

namespace Egram.Components.Chatting
{
    public class MessageAuthor : ReactiveObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private IBitmap _avatar;
        public IBitmap Avatar
        {
            get => _avatar;
            set => this.RaiseAndSetIfChanged(ref _avatar, value);
        }
    }
}