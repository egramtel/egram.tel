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

        private SourceList<ItemModel> _items;
        public ObservableCollectionExtended<ItemModel> Items { get; set; }
        
        public Tuple<int, int> VisibleIndexes { get; set; }
        
        public ExplorerModel(
            IMessageManager messageManager,
            IAvatarManager avatarManager,
            Target target
            )
        {
            var visibleRangeChanges = this.WhenAnyValue(m => m.VisibleIndexes)
                .Select(tuple => new Range(tuple?.Item1 ?? 0, tuple?.Item2 ?? 0))
                .SubscribeOn(TaskPoolScheduler.Default);
            
            BindMessages(target, messageManager, visibleRangeChanges)
                .DisposeWith(_modelDisposable);
            
            BindAvatars(avatarManager, visibleRangeChanges)
                .DisposeWith(_modelDisposable);
        }

        private IDisposable BindMessages(
            Target target,
            IMessageManager messageManager,
            IObservable<Range> visibleRangeChanges)
        {
            _items = new SourceList<ItemModel>();
            Items = new ObservableCollectionExtended<ItemModel>();
            
            var itemsSubscription = _items.Connect()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Items)
                .Subscribe();

            bool loaded = false;
            
            var loadingSubscription = visibleRangeChanges
                .SelectMany(range =>
                {
                    if (!loaded)
                    {
                        loaded = true;
                        
                        if (range.From == 0)
                        {
                            return messageManager.LoadPrevMessages(target, _items);
                        }
                        else if (range.To == _items.Count - 1)
                        {
                            return messageManager.LoadNextMessages(target, _items);
                        }
                    }

                    return Observable.Empty<Action>();
                })
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(action => action());
            
            return Disposable.Create(() =>
            {
                loadingSubscription.Dispose();
                itemsSubscription.Dispose();
            });
        }

        private IDisposable BindAvatars(
            IAvatarManager avatarManager,
            IObservable<Range> visibleRangeChanges)
        {
            var prevRange = new Range(0, 0);
            
            return visibleRangeChanges
                .SelectMany(range =>
                {
                    var releases = avatarManager.ReleaseAvatars(_items, prevRange, range);
                    var loads = avatarManager.LoadAvatars(_items, prevRange, range);

                    return releases.Concat(loads);
                })
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(action => action());
        }

        public void Dispose()
        {
            _modelDisposable.Dispose();
        }
    }
}