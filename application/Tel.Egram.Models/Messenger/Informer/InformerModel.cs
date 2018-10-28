using PropertyChanged;
using Tel.Egram.Graphics;

namespace Tel.Egram.Models.Messenger.Informer
{
    [AddINotifyPropertyChangedInterface]
    public class InformerModel
    {
        public bool IsVisible { get; set; } = true;
        
        public string Title { get; set; }
        
        public string Label { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public static InformerModel Hidden()
        {
            return new InformerModel
            {
                IsVisible = false
            };
        }
    }
}