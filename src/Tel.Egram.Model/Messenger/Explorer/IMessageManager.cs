using System;
using System.Collections.Generic;
using Tel.Egram.Model.Messenger.Explorer.Messages;
using Tel.Egram.Services.Messaging.Chats;
using Tel.Egram.Services.Messaging.Messages;

namespace Tel.Egram.Model.Messenger.Explorer
{
    public interface IMessageManager
    {
        IObservable<IList<MessageModel>> LoadPrevMessages(Chat chat, Message fromMessage = null);

        IObservable<IList<MessageModel>> LoadInitMessages(Chat chat, Message fromMessage = null);

        IObservable<IList<MessageModel>> LoadNextMessages(Chat chat, Message fromMessage = null);
    }
}