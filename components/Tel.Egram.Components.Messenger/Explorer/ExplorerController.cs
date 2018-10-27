using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger.Explorer;
using Tel.Egram.Models.Messenger.Explorer.Items;
using Tel.Egram.Models.Messenger.Explorer.Messages;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public class ExplorerController : BaseController<ExplorerModel>
    {
        private readonly SourceList<ItemModel> _items;
        
        public ExplorerController(
            Target target,
            IMessageManager messageManager,
            IAvatarManager avatarManager)
        {
            _items = new SourceList<ItemModel>();
            
            BindSource()
                .DisposeWith(this);
            
            BindVisibleRangeChanges(target, messageManager, avatarManager)
                .DisposeWith(this);
            
            InitMessageLoading(target, messageManager, avatarManager)
                .DisposeWith(this);
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
            var visibleRangeChanges = Model.WhenAnyValue(m => m.VisibleRange)
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

        private IDisposable BindSource()
        {
            var items = Model.Items;
            
            return _items.Connect()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(items)
                .Subscribe();
        }
    }
}