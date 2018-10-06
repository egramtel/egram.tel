using System;
using DynamicData;

namespace Tel.Egram.Components.Catalog
{
    public interface ICatalogProvider
    {   
        IObservableCache<EntryModel, long> Chats { get; }
    }
}