using PropertyChanged;
using Tel.Egram.Models.Messenger.Catalog;
using Tel.Egram.Models.Messenger.Editor;
using Tel.Egram.Models.Messenger.Explorer;
using Tel.Egram.Models.Messenger.Informer;

namespace Tel.Egram.Models.Messenger
{
    [AddINotifyPropertyChangedInterface]
    public class MessengerModel
    {
        public CatalogModel CatalogModel { get; set; }
        
        public InformerModel InformerModel { get; set; }
        
        public ExplorerModel ExplorerModel { get; set; }
        
        public EditorModel EditorModel { get; set; }
    }
}