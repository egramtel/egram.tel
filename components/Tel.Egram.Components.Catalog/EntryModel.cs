using Avalonia.Media;
using Avalonia.Media.Imaging;
using PropertyChanged;
using Tel.Egram.Graphics;

namespace Tel.Egram.Components.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public abstract class EntryModel
    {
        public string Title { get; set; }
        
        public Avatar Avatar { get; set; }
    }
}