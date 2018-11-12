using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Splat;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public static class ExplorerLogic
    {
        public static IDisposable BindSource(
            this ExplorerModel model)
        {   
            return model.SourceItems.Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(model.Items)
                .Subscribe();
        }

        public static IDisposable InitMessageLoading(
            this ExplorerModel model,
            Target target)
        {
            return InitMessageLoading(
                model,
                target,
                Locator.Current.GetService<IMessageManager>(),
                Locator.Current.GetService<IAvatarManager>());
        }

        public static IDisposable InitMessageLoading(
            this ExplorerModel model,
            Target target,
            IMessageManager messageManager,
            IAvatarManager avatarManager)
        {   
            var messageLoading = messageManager.LoadPrevMessages(target)
                .Select(models => new {
                    Action = new Action(() =>
                    {
                        model.SourceItems.InsertRange(models, 0);
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
            
            return model.SubscribeToActions(avatarLoading);
        }

        public static IDisposable BindVisibleRangeChanges(
            this ExplorerModel model,
            Target target)
        {
            return BindVisibleRangeChanges(
                model,
                target,
                Locator.Current.GetService<IMessageManager>(),
                Locator.Current.GetService<IAvatarManager>());
        }

        public static IDisposable BindVisibleRangeChanges(
            this ExplorerModel model,
            Target target,
            IMessageManager messageManager,
            IAvatarManager avatarManager)
        {
            var sourceItems = model.SourceItems;
            var prevRange = default(Range);
            var visibleRangeChanges = model.WhenAnyValue(m => m.VisibleRange)
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
                            var items = sourceItems.Items.OfType<MessageModel>().ToList();
                            var firstMessage = items.FirstOrDefault();
                            return messageManager.LoadPrevMessages(target, firstMessage?.Message)
                                .Select(models => new {
                                    Action = new Action(() =>
                                    {
                                        sourceItems.InsertRange(models, 0);
                                    }),
                                    Models = models
                                });
                        }
                        
                        if (item.Range.Index + item.Range.Length == sourceItems.Count
                            && item.Range.LastIndex != item.PrevRange.LastIndex)
                        {
                            var items = sourceItems.Items.OfType<MessageModel>().ToList();
                            var lastMessage = items.LastOrDefault();
                            return messageManager.LoadNextMessages(target, lastMessage?.Message)
                                .Select(models => new {
                                    Action = new Action(() =>
                                    {
                                        sourceItems.AddRange(models);
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

            return model.SubscribeToActions(avatarLoading);
        }

        private static IDisposable SubscribeToActions(
            this ExplorerModel model,
            IObservable<Action> actions)
        {
            return actions
                .Buffer(TimeSpan.FromMilliseconds(100))
                .SubscribeOn(RxApp.TaskpoolScheduler)
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
    }
}