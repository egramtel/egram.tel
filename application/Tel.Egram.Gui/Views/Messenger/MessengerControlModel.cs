using PropertyChanged;
using Tel.Egram.Gui.Views.Messenger.Catalog;
using Tel.Egram.Gui.Views.Messenger.Editor;
using Tel.Egram.Gui.Views.Messenger.Explorer;
using Tel.Egram.Gui.Views.Messenger.Informer;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Gui.Views.Messenger
{
    [AddINotifyPropertyChangedInterface]
    public class MessengerControlModel
    {
        public CatalogControlModel CatalogModel { get; set; }
        
        public InformerControlModel InformerModel { get; set; }
        
        public ExplorerControlModel ExplorerModel { get; set; }
        
        public EditorControlModel EditorModel { get; set; }
    }
}