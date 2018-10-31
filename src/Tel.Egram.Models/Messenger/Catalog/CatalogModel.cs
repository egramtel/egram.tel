using DynamicData.Binding;
using PropertyChanged;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger.Catalog.Entries;

namespace Tel.Egram.Models.Messenger.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public class CatalogModel
    {
        public bool IsVisible { get; set; } = true;
        
        public EntryModel SelectedEntry { get; set; }
        
        public ObservableCollectionExtended<EntryModel> Entries { get; set; }
            = new ObservableCollectionExtended<EntryModel>();
        
        public string SearchText { get; set; }

        public static CatalogModel Hidden()
        {
            return new CatalogModel
            {
                IsVisible = false
            };
        }
    }
}