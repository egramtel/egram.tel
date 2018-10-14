using TdLib;
using Tel.Egram.Components.Messenger.Explorer.Messages;
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
                default:
                    return CreateUnsupportedMessage(message);
            }
        }

        private MessageModel CreateTextMessage(Message message, TdApi.MessageContent.MessageText messageText)
        {
            var text = messageText.Text.Text;
            
            return new TextMessageModel
            {
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