using TdLib;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Model.Messenger.Explorer.Messages.Basic;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public interface IBasicMessageModelFactory
    {
        TextMessageModel CreateTextMessage(
            Message message,
            TdApi.MessageContent.MessageText messageText);

        MessageModel CreateUnsupportedMessage(Message message);
    }
}