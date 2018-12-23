using TdLib;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Components.Messenger.Explorer.Messages.Visual;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public partial class MessageModelFactory : IMessageModelFactory
    {
        private PhotoMessageModel CreatePhotoMessage(
            Message message,
            TdApi.MessageContent.MessagePhoto messagePhoto)
        {
            var user = message.User;
            var chat = message.Chat;

            var authorName = (user == null)
                ? chat.Title
                : $"{user.FirstName} {user.LastName}";
            
            var text = messagePhoto.Caption.Text;
            var photo = messagePhoto.Photo;
            
            return new PhotoMessageModel
            {
                AuthorName = authorName,
                Message = message,
                PhotoData = photo,
                Text = text
            };
        }

        private StickerMessageModel CreateStickerMessage(
            Message message,
            TdApi.MessageContent.MessageSticker messageSticker)
        {
            var user = message.User;
            var chat = message.Chat;

            var authorName = (user == null)
                ? chat.Title
                : $"{user.FirstName} {user.LastName}";
            
            var sticker = messageSticker.Sticker;
            
            return new StickerMessageModel
            {
                AuthorName = authorName,
                Message = message,
                StickerData = sticker
            };
        }

        private VideoMessageModel CreateVideoMessage(
            Message message,
            TdApi.MessageContent.MessageVideo messageVideo)
        {
            var user = message.User;
            var chat = message.Chat;

            var authorName = (user == null)
                ? chat.Title
                : $"{user.FirstName} {user.LastName}";
            
            var text = messageVideo.Caption.Text;
            var video = messageVideo.Video;
            
            return new VideoMessageModel
            {
                AuthorName = authorName,
                Message = message,
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