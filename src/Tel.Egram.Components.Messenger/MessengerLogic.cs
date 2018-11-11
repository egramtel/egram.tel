using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Catalog;
using Tel.Egram.Components.Messenger.Editor;
using Tel.Egram.Components.Messenger.Explorer;
using Tel.Egram.Components.Messenger.Informer;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger
{
    public static class MessengerLogic
    {
        public static IDisposable BindCatalog(this MessengerModel model, Section section)
        {
            model.CatalogModel = new CatalogModel(section);

            return Disposable.Empty;
        }

        public static IDisposable BindInformer(this MessengerModel model)
        {
            model.InformerModel = InformerModel.Hidden();
            
            return model.SubscribeToTarget(target =>
            {
                model.InformerModel = new InformerModel(target);
            });
        }

        public static IDisposable BindExplorer(this MessengerModel model)
        {
            model.ExplorerModel = ExplorerModel.Hidden();
            
            return model.SubscribeToTarget(target =>
            {
                model.ExplorerModel = new ExplorerModel(target);
            });
        }

        public static IDisposable BindEditor(this MessengerModel model)
        {
            model.EditorModel = EditorModel.Hidden();
            
            return model.SubscribeToTarget(target =>
            {
                if (target is Chat chat)
                {
                    model.EditorModel = new EditorModel(chat);
                }
                else
                {
                    model.EditorModel = EditorModel.Hidden();
                }
            });
        }

        private static IDisposable SubscribeToTarget(this MessengerModel model, Action<Target> action)
        {
            return model.WhenAnyValue(ctx => ctx.CatalogModel.SelectedEntry)
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(entry =>
                {
                    if (entry?.Target != null)
                    {
                        action(entry.Target);
                    }
                });
        }
    }
}