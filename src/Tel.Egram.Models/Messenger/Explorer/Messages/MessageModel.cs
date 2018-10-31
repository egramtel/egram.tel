using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Models.Messenger.Explorer.Items;

namespace Tel.Egram.Models.Messenger.Explorer.Messages
{
    public abstract class MessageModel : ItemModel
    {
        public string AuthorName { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public Message Message { get; set; }
    }
}
