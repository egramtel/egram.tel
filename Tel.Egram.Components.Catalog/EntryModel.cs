using Avalonia.Media;
using Avalonia.Media.Imaging;
using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Components.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public abstract class EntryModel
    {
        public bool IsFallbackAvatar { get; set; }
        public IBrush ColorBrush { get; set; }
        public IBitmap Avatar { get; set; }
        public string Title { get; set; }
        public string Init { get; set; }
    }
}