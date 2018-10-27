using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Gui.Views.Messenger.Informer
{
    public class InformerControlModel
    {
        public bool IsVisible { get; set; }
        
        public string Title { get; set; }
        
        public string Label { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public Target Target { get; set; }
        
        public static InformerControlModel Hidden()
        {
            return new InformerControlModel();
        }

        public static InformerControlModel FromTarget(Target target)
        {
            return new InformerControlModel
            {
                IsVisible = true,
                Target = target
            };
        }
    }
}