using Tel.Egram.Graphics;
using Tel.Egram.Gui.Views.Messenger.Explorer.Items;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Gui.Views.Messenger.Explorer.Messages
{
    public abstract class MessageModel : ItemModel
    {
        public string AuthorName { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public Message Message { get; set; }
    }
}
