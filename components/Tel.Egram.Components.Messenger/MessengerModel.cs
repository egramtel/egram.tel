using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Messenger.Editor;
using Tel.Egram.Components.Messenger.Explorer;
using Tel.Egram.Components.Messenger.Informer;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger
{
    [AddINotifyPropertyChangedInterface]
    public class MessengerModel : IDisposable
    {   
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();
        
        public CatalogModel CatalogModel { get; set; }
        
        public InformerModel InformerModel { get; set; }
        
        public ExplorerModel ExplorerModel { get; set; }
        
        public EditorModel EditorModel { get; set; }

        public MessengerModel(
            CatalogKind kind,
            IFactory<CatalogKind, CatalogModel> catalogModelFactory,
            IFactory<Target, InformerModel> informerModelFactory,
            IFactory<Target, ExplorerModel> explorerModelFactory,
            IFactory<Target, EditorModel> editorModelFactory
            )
        {
            BindCatalog(kind, catalogModelFactory);
            BindInformer(informerModelFactory).DisposeWith(_modelDisposable);
            BindExplorer(explorerModelFactory).DisposeWith(_modelDisposable);
            BindEditor(editorModelFactory).DisposeWith(_modelDisposable);
        }

        private void BindCatalog(
            CatalogKind kind,
            IFactory<CatalogKind, CatalogModel> catalogModelFactory
            )
        {
            CatalogModel = catalogModelFactory.Create(kind);
        }

        private IDisposable BindInformer(IFactory<Target, InformerModel> informerModelFactory)
        {
            var chatSubscription = SubscribeToSelectedChat(chat =>
            {
                InformerModel?.Dispose();
                InformerModel = informerModelFactory.Create(chat);
            });
            
            var aggregateSubscription = SubscribeToSelectedAggregate(aggregate =>
            {
                InformerModel?.Dispose();
                InformerModel = informerModelFactory.Create(aggregate);
            });
            
            return Disposable.Create(() =>
            {
                chatSubscription.Dispose();
                aggregateSubscription.Dispose();
            });
        }

        private IDisposable BindExplorer(IFactory<Target, ExplorerModel> explorerModelFactory)
        {
            var chatSubscription = SubscribeToSelectedChat(chat =>
            {
                ExplorerModel?.Dispose();
                ExplorerModel = explorerModelFactory.Create(chat);
            });
            
            var aggregateSubscription = SubscribeToSelectedAggregate(aggregate =>
            {
                ExplorerModel?.Dispose();
                ExplorerModel = explorerModelFactory.Create(aggregate);
            });
            
            return Disposable.Create(() =>
            {
                chatSubscription.Dispose();
                aggregateSubscription.Dispose();
            });
        }

        private IDisposable BindEditor(IFactory<Target, EditorModel> editorModelFactory)
        {
            var chatSubscription = SubscribeToSelectedChat(chat =>
            {
                EditorModel?.Dispose();
                EditorModel = editorModelFactory.Create(chat);
            });
            
            var aggregateSubscription = SubscribeToSelectedAggregate(aggregate =>
            {
                EditorModel?.Dispose();
                EditorModel = editorModelFactory.Create(aggregate);
            });
            
            return Disposable.Create(() =>
            {
                chatSubscription.Dispose();
                aggregateSubscription.Dispose();
            });
        }

        private IDisposable SubscribeToSelectedChat(Action<Chat> action)
        {
            return this.WhenAnyValue(ctx => ctx.CatalogModel.SelectedEntry)
                .OfType<ChatEntryModel>()
                .Select(e => e.Chat)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(action);
        }

        private IDisposable SubscribeToSelectedAggregate(Action<Aggregate> action)
        {
            return this.WhenAnyValue(ctx => ctx.CatalogModel.SelectedEntry)
                .OfType<AggregateEntryModel>()
                .Select(e => e.Aggregate)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(action);
        }

        public virtual void Dispose()
        {
            InformerModel?.Dispose();
            CatalogModel?.Dispose();
            ExplorerModel?.Dispose();
            EditorModel?.Dispose();
            
            _modelDisposable.Dispose();
        }
    }
}