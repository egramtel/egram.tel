using System.Reactive.Disposables;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Model.Messenger.Catalog;
using Tel.Egram.Model.Messenger.Editor;
using Tel.Egram.Model.Messenger.Explorer;
using Tel.Egram.Model.Messenger.Homepage;
using Tel.Egram.Model.Messenger.Informer;
using Tel.Egram.Services.Messaging.Chats;

namespace Tel.Egram.Model.Messenger
{
    [AddINotifyPropertyChangedInterface]
    public class MessengerModel : ISupportsActivation
    {   
        public CatalogModel CatalogModel { get; set; }
        
        public InformerModel InformerModel { get; set; }
        
        public ExplorerModel ExplorerModel { get; set; }
        
        public HomepageModel HomepageModel { get; set; }
        
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