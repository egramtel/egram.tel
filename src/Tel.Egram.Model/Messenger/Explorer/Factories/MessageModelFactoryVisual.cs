using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Model.Messenger.Explorer.Messages.Visual;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public partial class MessageModelFactory : IMessageModelFactory
    {
        private PhotoMessageModel CreatePhotoMessage(
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

        private StickerMessageModel CreateStickerMessage(
            Message message,
            TdApi.MessageContent.MessageSticker messageSticker)
        {
            var sticker = messageSticker.Sticker;
            
            return new StickerMessageModel
            {
                StickerData = sticker
            };
        }

        private VideoMessageModel CreateVideoMessage(
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

        private MessageModel CreateExpiredPhotoMessage(Message message, TdApi.MessageContent.MessageExpiredPhoto expiredPhoto)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateAnimationMessage(Message message, TdApi.MessageContent.MessageAnimation messageAnimation)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateExpiredVideoMessage(Message message, TdApi.MessageContent.MessageExpiredVideo expiredVideo)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }

        private MessageModel CreateVideoNoteMessage(Message message, TdApi.MessageContent.MessageVideoNote videoNote)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
    }
}