using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer.Factories
{
    public interface IMessageModelFactory
    {
        MessageModel CreateMessage(Message message);
    }
}