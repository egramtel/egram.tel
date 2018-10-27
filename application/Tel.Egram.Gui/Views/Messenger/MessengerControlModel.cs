using Tel.Egram.Gui.Views.Messenger.Catalog;
using Tel.Egram.Gui.Views.Messenger.Editor;
using Tel.Egram.Gui.Views.Messenger.Explorer;
using Tel.Egram.Gui.Views.Messenger.Informer;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Gui.Views.Messenger
{
    public class MessengerControlModel
    {
        public CatalogControlModel CatalogControlModel { get; set; }
        
        public InformerControlModel InformerControlModel { get; set; }
        
        public ExplorerControlModel ExplorerControlModel { get; set; }
        
        public EditorControlModel EditorControlModel { get; set; }
        
        public Section Section { get; set; }

        public static MessengerControlModel FromSection(Section section)
        {
            return new MessengerControlModel
            {
                Section = section
            };
        }
    }
}