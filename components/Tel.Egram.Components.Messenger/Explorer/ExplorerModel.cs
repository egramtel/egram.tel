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
            _items = new SourceList<ItemModel>();
            Items = new ObservableCollectionExtended<ItemModel>();
            
            _items.Connect()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(Items)
                .Subscribe()
                .DisposeWith(_modelDisposable);
            
            BindRangeChanges(target, messageManager, avatarManager)
                .DisposeWith(_modelDisposable);
        }

        private IDisposable BindRangeChanges(
            Target target,
            IMessageManager messageManager,
            IAvatarManager avatarManager)
        {
            var visibleRangeChanges = this.WhenAnyValue(m => m.VisibleIndexes)
                .Select(tuple => new Range(tuple?.Item1 ?? 0, tuple?.Item2 ?? 0))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler);
            
            var prevRange = new Range(0, 0);
            bool loaded = false;
            
            return visibleRangeChanges
                .SelectMany(range =>
                {
                    var messageLoads = Observable.Empty<Action>();
                    
                    if (!loaded)
                    {
                        loaded = true;
                        
                        if (range.From == 0)
                        {
                            messageLoads = messageLoads.Concat(
                                messageManager.LoadPrevMessages(target, _items));
                        }
                        else if (range.To == _items.Count - 1)
                        {
                            messageLoads = messageLoads.Concat(
                                messageManager.LoadNextMessages(target, _items));
                        }
                    }

                    //return messageLoads;

                    var avatarReleases = avatarManager.ReleaseAvatars(_items, prevRange, range);
                    var avatarLoads = avatarManager.LoadAvatars(_items, prevRange, range);

                    return messageLoads.Concat(avatarReleases).Concat(avatarLoads);
                })
                .Buffer(TimeSpan.FromMilliseconds(100))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    actions =>
                    {
                        foreach (var action in actions)
                        {
                            action();
                        }
                    },
                    error =>
                    {
                        Console.WriteLine(error);
                    });
        }

        public void Dispose()
        {
            _modelDisposable.Dispose();
        }
    }
}