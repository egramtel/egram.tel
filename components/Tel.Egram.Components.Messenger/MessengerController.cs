using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Messenger.Editor;
using Tel.Egram.Components.Messenger.Explorer;
using Tel.Egram.Components.Messenger.Informer;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger;
using Tel.Egram.Models.Messenger.Catalog;
using Tel.Egram.Models.Messenger.Editor;
using Tel.Egram.Models.Messenger.Explorer;
using Tel.Egram.Models.Messenger.Informer;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger
{
    public class MessengerController : BaseController<MessengerModel>
    {
        private readonly IActivator<Section, CatalogModel> _catalogActivator;
        private IController<CatalogModel> _catalogController;
        
        private readonly IActivator<Target, InformerModel> _informerActivator;
        private IController<InformerModel> _informerController;
        
        private readonly IActivator<Target, ExplorerModel> _explorerActivator;
        private IController<ExplorerModel> _explorerController;
        
        private readonly IActivator<Target, EditorModel> _editorActivator;
        private IController<EditorModel> _editorController;

        public MessengerController(
            Section section,
            IActivator<Section, CatalogModel> catalogActivator,
            IActivator<Target, InformerModel> informerActivator,
            IActivator<Target, ExplorerModel> explorerActivator,
            IActivator<Target, EditorModel> editorActivator)
        {
            _catalogActivator = catalogActivator;
            _informerActivator = informerActivator;
            _explorerActivator = explorerActivator;
            _editorActivator = editorActivator;

            BindCatalog(section).DisposeWith(this);
            BindInformer().DisposeWith(this);
            BindExplorer().DisposeWith(this);
            BindEditor().DisposeWith(this);
        }

        private IDisposable BindCatalog(Section section)
        {
            _catalogActivator.Deactivate(ref _catalogController);
            var model = _catalogActivator.Activate(section, ref _catalogController);
            Model.CatalogModel = model;

            return Disposable.Empty;
        }

        private IDisposable BindInformer()
        {
            Model.InformerModel = InformerModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _informerActivator.Deactivate(ref _informerController);
                var model = _informerActivator.Activate(target, ref _informerController);
                Model.InformerModel = model;
            });
        }

        private IDisposable BindExplorer()
        {
            Model.ExplorerModel = ExplorerModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _explorerActivator.Deactivate(ref _explorerController);
                var model = _explorerActivator.Activate(target, ref _explorerController);
                Model.ExplorerModel = model;
            });
        }

        private IDisposable BindEditor()
        {
            Model.EditorModel = EditorModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _editorActivator.Deactivate(ref _editorController);
                var model = _editorActivator.Activate(target, ref _editorController);
                Model.EditorModel = model;
            });
        }

        private IDisposable SubscribeToTarget(Action<Target> action)
        {
            return Model.WhenAnyValue(ctx => ctx.CatalogModel.SelectedEntry)
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(entry =>
                {
                    if (entry?.Target != null)
                    {
                        action(entry.Target);
                    }
                });
        }

        public override void Dispose()
        {
            _catalogActivator.Deactivate(ref _catalogController);
            _informerActivator.Deactivate(ref _informerController);
            _explorerActivator.Deactivate(ref _explorerController);
            _editorActivator.Deactivate(ref _editorController);
            
            base.Dispose();
        }
    }
}