using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Model.Messenger.Explorer.Messages.Visual;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public class VisualMessageModelFactory : IVisualMessageModelFactory
    {
        public PhotoMessageModel CreatePhotoMessage(
            Message message,
            TdApi.MessageContent.MessagePhoto messagePhoto)
        {
            var text = messagePhoto.Caption.Text;
            var photo = messagePhoto.Photo;
            
            return new PhotoMessageModel
            {
                PhotoData = photo,
                Text = text
            };
        }

        public StickerMessageModel CreateStickerMessage(
            Message message,
            TdApi.MessageContent.MessageSticker messageSticker)
        {
            var sticker = messageSticker.Sticker;
            
            return new StickerMessageModel
            {
                StickerData = sticker
            };
        }

        public VideoMessageModel CreateVideoMessage(
            Message message,
            TdApi.MessageContent.MessageVideo messageVideo)
        {
            var text = messageVideo.Caption.Text;
            var video = messageVideo.Video;
            
            return new VideoMessageModel
            {
                VideoData = video,
                Text = text
            };
        }

        public MessageModel CreateExpiredPhotoMessage(Message message, TdApi.MessageContent.MessageExpiredPhoto expiredPhoto)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateAnimationMessage(Message message, TdApi.MessageContent.MessageAnimation messageAnimation)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateExpiredVideoMessage(Message message, TdApi.MessageContent.MessageExpiredVideo expiredVideo)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        public MessageModel CreateVideoNoteMessage(Message message, TdApi.MessageContent.MessageVideoNote videoNote)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
    }
}