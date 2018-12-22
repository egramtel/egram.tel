using TdLib;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public class MessageModelFactory : IMessageModelFactory
    {
        public MessageModel CreateMessage(Message message)
        {
            var messageData = message.MessageData;
            var content = messageData.Content;

            switch (content)
            {
                case TdApi.MessageContent.MessageText messageText:
                    return CreateTextMessage(message, messageText);
                
                case TdApi.MessageContent.MessagePhoto messagePhoto:
                    return CreatePhotoMessage(message, messagePhoto);
                
//                case TdApi.MessageContent.MessageSticker messageSticker:
//                    return CreateStickerMessage(message, messageSticker);
                
//                case TdApi.MessageContent.MessageVideo messageVideo:
//                    return CreateVideoMessage(message, messageVideo);
                
                default:
                    return CreateUnsupportedMessage(message);
            }
        }

        private TextMessageModel CreateTextMessage(
            Message message,
            TdApi.MessageContent.MessageText messageText)
        {
            var user = message.User;
            var chat = message.Chat;

            var authorName = (user == null)
                ? chat.Title
                : $"{user.FirstName} {user.LastName}";
            
            var text = messageText.Text.Text;
            
            return new TextMessageModel
            {
                AuthorName = authorName,
                Message = message,
                Text = text
            };
        }

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
                Photo = photo,
                Text = text
            };
        }

        private StickerMessageModel CreateStickerMessage(
            Message message,
            TdApi.MessageContent.MessageSticker messageSticker)
        {
            throw new System.NotImplementedException();
        }

        private VideoMessageModel CreateVideoMessage(
            Message message,
            TdApi.MessageContent.MessageVideo messageVideo)
        {
            throw new System.NotImplementedException();
        }

        private MessageModel CreateUnsupportedMessage(Message message)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
    }
}