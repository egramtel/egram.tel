using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Explorer.Triggers;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    [AddINotifyPropertyChangedInterface]
    public class ExplorerModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();

        public ObservableCollectionExtended<ItemModel> Items { get; set; }
        
        public Tuple<int, int> VisibleIndexes { get; set; }
        
        public ExplorerModel(
            IFactory<Target, ExplorerProvider> explorerProviderFactory,
            IExplorerTrigger explorerTrigger,
            Target target
            )
        {
            var explorerProvider = explorerProviderFactory.Create(target);
            BindMessages(explorerProvider).DisposeWith(_modelDisposable);

            BindLoading(explorerTrigger).DisposeWith(_modelDisposable);

            BindVisibility(explorerTrigger).DisposeWith(_modelDisposable);
        }

        private IDisposable BindMessages(ExplorerProvider explorerProvider)
        {
            Items = new ObservableCollectionExtended<ItemModel>();
            
            var itemsSubscription = explorerProvider.Items.Connect()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Items)
                .Subscribe();
            
            return Disposable.Create(() =>
            {
                itemsSubscription.Dispose();
                explorerProvider.Dispose();
            });
        }

        private IDisposable BindLoading(IExplorerTrigger explorerTrigger)
        {
            explorerTrigger.LoadMessages(LoadDirection.Prev);
            return Disposable.Empty;
        }

        private IDisposable BindVisibility(IExplorerTrigger explorerTrigger)
        {
            return this.WhenAnyValue(m => m.VisibleIndexes)
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(tuple =>
                {
                    if (tuple != null)
                    {
                        explorerTrigger.NotifyVisibleRange(tuple.Item1, tuple.Item2);
                    }
                });
        }

        public void Dispose()
        {
            _modelDisposable.Dispose();
        }
    }
}