using DynamicData;
using Tel.Egram.Model.Messenger.Catalog.Entries;

namespace Tel.Egram.Model.Messenger.Catalog
{
    public interface ICatalogProvider
    {   
        IObservableCache<EntryModel, long> Chats { get; }
    }
}