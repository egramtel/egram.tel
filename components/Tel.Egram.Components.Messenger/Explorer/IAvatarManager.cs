using System;
using System.Reactive;
using DynamicData;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IAvatarManager
    {
        IObservable<Action> ReleaseAvatars(SourceList<ItemModel> items, Range prevRange, Range range);
        IObservable<Action> LoadAvatars(SourceList<ItemModel> items, Range prevRange, Range range);
    }
}