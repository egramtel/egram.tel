using PropertyChanged;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Gui.Views.Messenger.Informer
{
    [AddINotifyPropertyChangedInterface]
    public class InformerControlModel
    {
        public bool IsVisible { get; set; }
        
        public string Title { get; set; }
        
        public string Label { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public static InformerControlModel Hidden()
        {
            return new InformerControlModel();
        }
    }
}