using Tel.Egram.Components.Messenger.Explorer.Items;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer.Messages
{
    public abstract class MessageModel : ItemModel
    {
        public string AuthorName { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public Message Message { get; set; }
    }
}
