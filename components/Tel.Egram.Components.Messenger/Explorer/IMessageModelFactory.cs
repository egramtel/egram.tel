using System.Collections.Generic;
using Tel.Egram.Gui.Views.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IMessageModelFactory
    {
        MessageModel CreateMessage(Message message);
    }
}