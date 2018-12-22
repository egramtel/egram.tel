using System;
using System.Collections.Generic;
using DynamicData;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IMessageManager
    {
        IObservable<IList<MessageModel>> LoadPrevMessages(Chat chat, Message fromMessage = null);

        IObservable<IList<MessageModel>> LoadInitMessages(Chat chat, Message fromMessage = null);

        IObservable<IList<MessageModel>> LoadNextMessages(Chat chat, Message fromMessage = null);
    }
}