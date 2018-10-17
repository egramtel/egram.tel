using System;
using DynamicData;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IMessageManager
    {
        IDisposable LoadPrevMessages(Target target, SourceList<ItemModel> items);
        IDisposable LoadNextMessages(Target target, SourceList<ItemModel> items);
    }
}