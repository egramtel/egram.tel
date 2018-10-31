using System;
using System.Collections.Generic;
using DynamicData;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Models.Messenger.Explorer.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IMessageManager
    {
        IObservable<IList<MessageModel>> LoadPrevMessages(Target target, Message fromMessage);
        IObservable<IList<MessageModel>> LoadPrevMessages(Target target);
        IObservable<IList<MessageModel>> LoadNextMessages(Target target, Message fromMessage);
        IObservable<IList<MessageModel>> LoadNextMessages(Target target);
    }
}