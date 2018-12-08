using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Messenger.Editor;
using Tel.Egram.Components.Messenger.Explorer;
using Tel.Egram.Components.Messenger.Home;
using Tel.Egram.Components.Messenger.Informer;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Components.Messenger
{
    [AddINotifyPropertyChangedInterface]
    public class MessengerModel : ISupportsActivation
    {   
        public CatalogModel CatalogModel { get; set; }
        
        public InformerModel InformerModel { get; set; }
        
        public ExplorerModel ExplorerModel { get; set; }
        
        public HomeModel HomeModel { get; set; }
        
        public EditorModel EditorModel { get; set; }

        public MessengerModel(Section section)
        {
            this.WhenActivated(disposables =>
            {
                this.BindCatalog(section)
                    .DisposeWith(disposables);

                this.BindInformer()
                    .DisposeWith(disposables);

                this.BindExplorer()
                    .DisposeWith(disposables);

                this.BindHome()
                    .DisposeWith(disposables);

                this.BindEditor()
                    .DisposeWith(disposables);

                this.BindNotifications()
                    .DisposeWith(disposables);
            });
        }
        
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}