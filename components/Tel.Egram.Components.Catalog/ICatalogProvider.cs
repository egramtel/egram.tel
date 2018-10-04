using System;
using DynamicData;

namespace Tel.Egram.Components.Catalog
{
    public interface ICatalogProvider
    {
        bool BotFilter(EntryModel model);
        
        bool DirectFilter(EntryModel model);
        
        bool GroupFilter(EntryModel model);
        
        bool ChannelFilter(EntryModel model);
        
        IObservableCache<EntryModel, long> Chats { get; }
    }
}