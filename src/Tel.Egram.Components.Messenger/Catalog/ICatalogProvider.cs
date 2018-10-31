using DynamicData;
using Tel.Egram.Models.Messenger.Catalog.Entries;

namespace Tel.Egram.Components.Messenger.Catalog
{
    public interface ICatalogProvider
    {   
        IObservableCache<EntryModel, long> Chats { get; }
    }
}