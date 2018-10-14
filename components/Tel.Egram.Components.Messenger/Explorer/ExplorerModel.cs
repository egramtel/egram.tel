using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    [AddINotifyPropertyChangedInterface]
    public class ExplorerModel : IDisposable
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();

        public ObservableCollectionExtended<ItemModel> Items { get; set; }
        
        public ExplorerModel(
            IFactory<Target, ExplorerProvider> explorerProviderFactory,
            IExplorerTrigger explorerTrigger,
            Target target
            )
        {
            var explorerProvider = explorerProviderFactory.Create(target);
            BindMessages(explorerProvider).DisposeWith(_modelDisposable);

            BindTriggers(explorerTrigger).DisposeWith(_modelDisposable);
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

        private IDisposable BindTriggers(IExplorerTrigger explorerTrigger)
        {
            return explorerTrigger.Trigger(new ExplorerSignal.LoadPrev());
        }

        public void Dispose()
        {
            _modelDisposable.Dispose();
        }
    }
}