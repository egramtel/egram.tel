using System.Reactive.Disposables;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Explorer.Items;
using Tel.Egram.Graphics;
using Tel.Egram.Graphics.Avatars;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer.Messages
{
    public abstract class MessageModel : ItemModel
    {
        public string AuthorName { get; set; }

        public string Time { get; set; }
        
        public Avatar Avatar { get; set; }
        
        public Message Message { get; set; }
    }
}
