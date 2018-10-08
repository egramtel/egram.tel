using DynamicData;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public interface ICatalogProvider
    {   
        IObservableCache<EntryModel, long> Chats { get; }
    }
}