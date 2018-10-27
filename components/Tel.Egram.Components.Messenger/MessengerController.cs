using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Messenger.Editor;
using Tel.Egram.Components.Messenger.Explorer;
using Tel.Egram.Components.Messenger.Informer;
using Tel.Egram.Gui.Views.Messenger;
using Tel.Egram.Gui.Views.Messenger.Catalog;
using Tel.Egram.Gui.Views.Messenger.Editor;
using Tel.Egram.Gui.Views.Messenger.Explorer;
using Tel.Egram.Gui.Views.Messenger.Informer;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger
{
    public class MessengerController
        : BaseController, IMessengerController
    {
        private readonly IFactory<CatalogControlModel, ICatalogController> _catalogControllerFactory;
        private ICatalogController _catalogController;
        
        private readonly IFactory<InformerControlModel, IInformerController> _informerControllerFactory;
        private IInformerController _informerController;
        
        private readonly IFactory<ExplorerControlModel, IExplorerController> _explorerControllerFactory;
        private IExplorerController _explorerController;
        
        private readonly IFactory<EditorControlModel, IEditorController> _editorControllerFactory;
        private IEditorController _editorController;

        public MessengerController(
            MessengerControlModel messengerModel,
            IFactory<CatalogControlModel, ICatalogController> catalogControllerFactory,
            IFactory<InformerControlModel, IInformerController> informerControllerFactory,
            IFactory<ExplorerControlModel, IExplorerController> explorerControllerFactory,
            IFactory<EditorControlModel, IEditorController> editorControllerFactory)
        {
            _catalogControllerFactory = catalogControllerFactory;
            _informerControllerFactory = informerControllerFactory;
            _explorerControllerFactory = explorerControllerFactory;
            _editorControllerFactory = editorControllerFactory;

            BindCatalog(messengerModel)
                .DisposeWith(this);
            
            BindInformer(messengerModel)
                .DisposeWith(this);
            
            BindExplorer(messengerModel)
                .DisposeWith(this);
            
            BindEditor(messengerModel)
                .DisposeWith(this);
        }

        private IDisposable BindCatalog(MessengerControlModel model)
        {
            var catalogModel = new CatalogControlModel();
            
            model.CatalogControlModel = catalogModel;

            _catalogController?.Dispose();
            _catalogController = _catalogControllerFactory.Create(catalogModel);

            return Disposable.Empty;
        }

        private IDisposable BindInformer(MessengerControlModel model)
        {
            model.InformerControlModel = InformerControlModel.Hidden();
            
            return SubscribeToTarget(model, target =>
            {
                var informerModel = InformerControlModel.FromTarget(target);
                model.InformerControlModel = informerModel;

                _informerController?.Dispose();
                _informerController = _informerControllerFactory.Create(informerModel);
            });
        }

        private IDisposable BindExplorer(MessengerControlModel model)
        {
            model.ExplorerControlModel = ExplorerControlModel.Hidden();
            
            return SubscribeToTarget(model, target =>
            {
                var explorerModel = ExplorerControlModel.FromTarget(target);
                model.ExplorerControlModel = explorerModel;

                _explorerController?.Dispose();
                _explorerController = _explorerControllerFactory.Create(explorerModel);
            });
        }

        private IDisposable BindEditor(MessengerControlModel model)
        {
            model.EditorControlModel = EditorControlModel.Hidden();
            
            return SubscribeToTarget(model, target =>
            {
                var editorModel = EditorControlModel.FromTarget(target);
                model.EditorControlModel = editorModel;

                _editorController?.Dispose();
                _editorController = _editorControllerFactory.Create(editorModel);
            });
        }

        private IDisposable SubscribeToTarget(MessengerControlModel model, Action<Target> action)
        {
            return model.WhenAnyValue(ctx => ctx.CatalogControlModel.SelectedEntry)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(entry =>
                {
                    action(entry.Target);
                });
        }

        public override void Dispose()
        {
            _catalogController?.Dispose();
            _informerController?.Dispose();
            _explorerController?.Dispose();
            _editorController?.Dispose();
            
            base.Dispose();
        }
    }
}