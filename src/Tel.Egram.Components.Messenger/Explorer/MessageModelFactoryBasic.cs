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
            var text = messageText.Text.Text;
            
            return new TextMessageModel
            {
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