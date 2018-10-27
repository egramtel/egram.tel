using DynamicData.Binding;
using PropertyChanged;
using Tel.Egram.Gui.Views.Messenger.Catalog.Entries;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Gui.Views.Messenger.Catalog
{
    [AddINotifyPropertyChangedInterface]
    public class CatalogControlModel
    {
        public EntryModel SelectedEntry { get; set; }
        
        public ObservableCollectionExtended<EntryModel> Entries { get; set; }
        
        public string SearchText { get; set; }

        public Section Section { get; set; }

        public static CatalogControlModel FromSection(Section section)
        {
            return new CatalogControlModel
            {
                Entries = new ObservableCollectionExtended<EntryModel>(),
                Section = section
            };
        }
    }
}