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
        : BaseController<MessengerControlModel>, IMessengerController
    {
        private readonly IFactory<Section, ICatalogController> _catalogControllerFactory;
        private ICatalogController _catalogController;
        
        private readonly IFactory<Target, IInformerController> _informerControllerFactory;
        private IInformerController _informerController;
        
        private readonly IFactory<Target, IExplorerController> _explorerControllerFactory;
        private IExplorerController _explorerController;
        
        private readonly IFactory<Target, IEditorController> _editorControllerFactory;
        private IEditorController _editorController;

        public MessengerController(
            Section section,
            IFactory<Section, ICatalogController> catalogControllerFactory,
            IFactory<Target, IInformerController> informerControllerFactory,
            IFactory<Target, IExplorerController> explorerControllerFactory,
            IFactory<Target, IEditorController> editorControllerFactory)
        {
            _catalogControllerFactory = catalogControllerFactory;
            _informerControllerFactory = informerControllerFactory;
            _explorerControllerFactory = explorerControllerFactory;
            _editorControllerFactory = editorControllerFactory;

            BindCatalog(section).DisposeWith(this);
            BindInformer().DisposeWith(this);
            BindExplorer().DisposeWith(this);
            BindEditor().DisposeWith(this);
        }

        private IDisposable BindCatalog(Section section)
        {
            _catalogController?.Dispose();
            _catalogController = _catalogControllerFactory.Create(section);
            Model.CatalogControlModel = _catalogController.Model;

            return Disposable.Empty;
        }

        private IDisposable BindInformer()
        {
            Model.InformerControlModel = InformerControlModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _informerController?.Dispose();
                _informerController = _informerControllerFactory.Create(target);
                Model.InformerControlModel = _informerController.Model;
            });
        }

        private IDisposable BindExplorer()
        {
            Model.ExplorerControlModel = ExplorerControlModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _explorerController?.Dispose();
                _explorerController = _explorerControllerFactory.Create(target);
                Model.ExplorerControlModel = _explorerController.Model;
            });
        }

        private IDisposable BindEditor()
        {
            Model.EditorControlModel = EditorControlModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _editorController?.Dispose();
                _editorController = _editorControllerFactory.Create(target);
                Model.EditorControlModel = _editorController.Model;
            });
        }

        private IDisposable SubscribeToTarget(Action<Target> action)
        {
            return Model.WhenAnyValue(ctx => ctx.CatalogControlModel.SelectedEntry)
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