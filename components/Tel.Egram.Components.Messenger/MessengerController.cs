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
    public class MessengerController : BaseController<MessengerControlModel>
    {
        private readonly IActivator<Section, CatalogControlModel> _catalogActivator;
        private IController<CatalogControlModel> _catalogController;
        
        private readonly IActivator<Target, InformerControlModel> _informerActivator;
        private IController<InformerControlModel> _informerController;
        
        private readonly IActivator<Target, ExplorerControlModel> _explorerActivator;
        private IController<ExplorerControlModel> _explorerController;
        
        private readonly IActivator<Target, EditorControlModel> _editorActivator;
        private IController<EditorControlModel> _editorController;

        public MessengerController(
            Section section,
            IActivator<Section, CatalogControlModel> catalogActivator,
            IActivator<Target, InformerControlModel> informerActivator,
            IActivator<Target, ExplorerControlModel> explorerActivator,
            IActivator<Target, EditorControlModel> editorActivator)
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
            _catalogController?.Dispose();
            var model = _catalogActivator.Activate(section, ref _catalogController);
            Model.CatalogModel = model;

            return Disposable.Empty;
        }

        private IDisposable BindInformer()
        {
            Model.InformerModel = InformerControlModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _informerController?.Dispose();
                var model = _informerActivator.Activate(target, ref _informerController);
                Model.InformerModel = model;
            });
        }

        private IDisposable BindExplorer()
        {
            Model.ExplorerModel = ExplorerControlModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _explorerController?.Dispose();
                var model = _explorerActivator.Activate(target, ref _explorerController);
                Model.ExplorerModel = model;
            });
        }

        private IDisposable BindEditor()
        {
            Model.EditorModel = EditorControlModel.Hidden();
            
            return SubscribeToTarget(target =>
            {
                _editorController?.Dispose();
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