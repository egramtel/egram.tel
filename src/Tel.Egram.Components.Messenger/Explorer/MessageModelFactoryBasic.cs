using TdLib;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Components.Messenger.Explorer.Messages.Basic;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public partial class MessageModelFactory : IMessageModelFactory
    {
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

        private MessageModel CreateUnsupportedMessage(Message message)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
    }
}