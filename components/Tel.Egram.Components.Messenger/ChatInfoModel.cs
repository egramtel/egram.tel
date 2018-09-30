using Avalonia.Media;
using Avalonia.Media.Imaging;
using PropertyChanged;
using Tel.Egram.Graphics;

namespace Tel.Egram.Components.Messenger
{
    [AddINotifyPropertyChangedInterface]
    public class ChatInfoModel
    {
        public string Title { get; set; }
        
        public string Label { get; set; }
        
        public Avatar Avatar { get; set; }
    }
}