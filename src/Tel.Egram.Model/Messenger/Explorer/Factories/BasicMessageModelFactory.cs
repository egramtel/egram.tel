using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Model.Messenger.Explorer.Messages.Basic;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public class BasicMessageModelFactory : IBasicMessageModelFactory
    {
        public BasicMessageModelFactory()
        {
            
        }
        
        public TextMessageModel CreateTextMessage(
            Message message,
            TdApi.MessageContent.MessageText messageText)
        {
            var text = messageText.Text.Text;
            
            return new TextMessageModel
            {
                Text = text
            };
        }

        public MessageModel CreateUnsupportedMessage(Message message)
        {
            return new UnsupportedMessageModel
            {
                Message = message
            };
        }
    }
}