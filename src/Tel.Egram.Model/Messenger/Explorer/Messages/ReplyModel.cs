using PropertyChanged;
using TdLib;
using Tel.Egram.Services.Graphics.Previews;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Messages
{
    [AddINotifyPropertyChangedInterface]
    public class ReplyModel
    {
        public string AuthorName { get; set; }
        
        public string Text { get; set; }
        
        public bool HasPreview { get; set; }
        
        public Preview Preview { get; set; }
        
        public Message Message { get; set; }
        
        public TdApi.Photo PhotoData { get; set; }
        
        public TdApi.Sticker StickerData { get; set; }
        
        public TdApi.Video VideoData { get; set; }
    }
}