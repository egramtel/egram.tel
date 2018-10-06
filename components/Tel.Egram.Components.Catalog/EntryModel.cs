using Avalonia.Media;
using Avalonia.Media.Imaging;
using PropertyChanged;
using Tel.Egram.Graphics;

namespace Tel.Egram.Components.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public abstract class EntryModel
    {
        public long Id { get; set; }
        
        public int Order { get; set; }
        
        public string Title { get; set; }
        
        public Avatar Avatar { get; set; }

        public bool HasUnread { get; set; }

        public string UnreadCount { get; set; }
    }
}