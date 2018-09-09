using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace Tel.Egram.Components.Catalog
{
    public abstract class EntryModel : ReactiveObject
    {
        private IBitmap _avatar;
        public IBitmap Avatar
        {
            get => _avatar;
            set => this.RaiseAndSetIfChanged(ref _avatar, value);
        }

        private IBrush _colorBrush;
        public IBrush ColorBrush
        {
            get => _colorBrush;
            set => this.RaiseAndSetIfChanged(ref _colorBrush, value);
        }

        private bool _isFallbackAvatar;
        public bool IsFallbackAvatar
        {
            get => _isFallbackAvatar;
            set => this.RaiseAndSetIfChanged(ref _isFallbackAvatar, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private string _init;
        public string Init
        {
            get => _init;
            set => this.RaiseAndSetIfChanged(ref _init, value);
        }
    }
}