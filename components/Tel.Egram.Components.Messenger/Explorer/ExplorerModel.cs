using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PropertyChanged;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Explorer.Messages;
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
        
        public Range VisibleRange { get; set; }
        
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
            
            BindVisibleRangeChanges(target, messageManager, avatarManager)
                .DisposeWith(_modelDisposable);
            
            InitMessageLoading(target, messageManager, avatarManager)
                .DisposeWith(_modelDisposable);
        }

        private IDisposable InitMessageLoading(
            Target target,
            IMessageManager messageManager,
            IAvatarManager avatarManager)
        {
            var messageLoading = messageManager.LoadPrevMessages(target)
                .Select(models => new {
                    Action = new Action(() =>
                    {
                        _items.InsertRange(models, 0);
                    }),
                    Models = models
                });

            var avatarLoading = messageLoading
                .SelectMany(item =>
                {
                    var action = item.Action;
                    var models = item.Models;
                    
                    var messageLoadAction = Observable.Return(action);
                    
                    var avatarPreloadAction = avatarManager.PreloadAvatars(models)
                        .Select((avatar, i) => new Action(() =>
                        {
                            var messageModel = models[i];
                            messageModel.Avatar = avatar;
                        }));
                    
                    var avatarLoadAction = avatarManager.LoadAvatars(models)
                        .Select((avatar, i) => new Action(() =>
                        {
                            var messageModel = models[i];
                            messageModel.Avatar = avatar;
                        }));

                    return messageLoadAction
                        .Concat(avatarPreloadAction)
                        .Concat(avatarLoadAction);
                });
            
            return SubscribeToActions(avatarLoading);
        }

        private IDisposable BindVisibleRangeChanges(
            Target target,
            IMessageManager messageManager,
            IAvatarManager avatarManager)
        {
            var prevRange = default(Range);
            var visibleRangeChanges = this.WhenAnyValue(m => m.VisibleRange)
                .Select(range => new
                {
                    PrevRange = prevRange,
                    Range = range
                })
                .Do(item => prevRange = item.Range)
                .Do(Console.WriteLine);

            var messageLoading = visibleRangeChanges
                .SelectMany(item =>
                {
                    if (item.Range.Length > 0)
                    {
                        if (item.Range.Index == 0
                            && item.Range.Index != item.PrevRange.Index)
                        {
                            var items = _items.Items.OfType<MessageModel>().ToList();
                            var firstMessage = items.FirstOrDefault();
                            return messageManager.LoadPrevMessages(target, firstMessage?.Message)
                                .Select(models => new {
                                    Action = new Action(() =>
                                    {
                                        _items.InsertRange(models, 0);
                                    }),
                                    Models = models
                                });
                        }
                        
                        if (item.Range.Index + item.Range.Length == _items.Count
                            && item.Range.LastIndex != item.PrevRange.LastIndex)
                        {
                            var items = _items.Items.OfType<MessageModel>().ToList();
                            var lastMessage = items.LastOrDefault();
                            return messageManager.LoadNextMessages(target, lastMessage?.Message)
                                .Select(models => new {
                                    Action = new Action(() =>
                                    {
                                        _items.AddRange(models);
                                    }),
                                    Models = models
                                });
                        }
                    }

                    return Observable.Empty<IList<MessageModel>>()
                        .Select(models => new
                        {
                            Action = new Action(() => { }),
                            Models = models
                        });
                });
            
            var avatarLoading = messageLoading
                .SelectMany(item =>
                {
                    var action = item.Action;
                    var models = item.Models;
                    
                    var messageLoadAction = Observable.Return(action);
                    
                    var avatarPreloadAction = avatarManager.PreloadAvatars(models)
                        .Select((avatar, i) => new Action(() =>
                        {
                            var messageModel = models[i];
                            messageModel.Avatar = avatar;
                        }));
                    
                    var avatarLoadAction = avatarManager.LoadAvatars(models)
                        .Select((avatar, i) => new Action(() =>
                        {
                            var messageModel = models[i];
                            messageModel.Avatar = avatar;
                        }));

                    return messageLoadAction
                        .Concat(avatarPreloadAction)
                        .Concat(avatarLoadAction);
                });

            return SubscribeToActions(avatarLoading);
        }

        private IDisposable SubscribeToActions(IObservable<Action> actions)
        {
            return actions
                .Buffer(TimeSpan.FromMilliseconds(100))
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    actionList =>
                    {
                        foreach (var action in actionList)
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