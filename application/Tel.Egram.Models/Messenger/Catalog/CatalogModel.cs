using DynamicData.Binding;
using PropertyChanged;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger.Catalog.Entries;

namespace Tel.Egram.Models.Messenger.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public class CatalogModel
    {
        public EntryModel SelectedEntry { get; set; }
        
        public ObservableCollectionExtended<EntryModel> Entries { get; set; }
        
        public string SearchText { get; set; }
    }
}