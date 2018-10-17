using System;
using DynamicData;
using Tel.Egram.Components.Messenger.Explorer.Triggers;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public interface IAvatarManager
    {
        IDisposable ReleaseAvatars(SourceList<ItemModel> items, Range prevRange, Range range);
        IDisposable LoadAvatars(SourceList<ItemModel> items, Range prevRange, Range range);
    }
}