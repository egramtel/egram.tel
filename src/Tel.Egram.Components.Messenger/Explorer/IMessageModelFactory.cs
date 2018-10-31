using System.Collections.Generic;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Models.Messenger.Explorer.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IMessageModelFactory
    {
        MessageModel CreateMessage(Message message);
    }
}