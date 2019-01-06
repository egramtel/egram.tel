using Tel.Egram.Model.Messenger.Explorer.Items;
using Tel.Egram.Services.Graphics.Avatars;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Messages
{
    public abstract class MessageModel : ItemModel
    {
        public string AuthorName { get; set; }

        public string Time { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public Message Message { get; set; }
        
        public bool HasReply { get; set; }
        
        public ReplyModel Reply { get; set; }
    }
}
