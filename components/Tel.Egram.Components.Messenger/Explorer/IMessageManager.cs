using System;
using DynamicData;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IMessageManager
    {
        IObservable<Action> LoadPrevMessages(Target target, SourceList<ItemModel> items);
        IObservable<Action> LoadNextMessages(Target target, SourceList<ItemModel> items);
    }
}