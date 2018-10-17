using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Components.Messenger.Explorer.Triggers;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public class ExplorerProvider : IExplorerProvider, IDisposable
    {   
        private readonly CompositeDisposable _serviceDisposable = new CompositeDisposable();
        
        private readonly SourceList<ItemModel> _items;
        public IObservableList<ItemModel> Items => _items;

        public ExplorerProvider(
            Target target,
            IExplorerTrigger trigger,
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory,
            IAvatarLoader avatarLoader
            )
        {
            _items = new SourceList<ItemModel>();

            var loadRequests = Observable.FromEventPattern<MessageLoadRequestedArgs>(
                h => trigger.MessageLoadRequested += h,
                h => trigger.MessageLoadRequested -= h)
                .Select(args => args.EventArgs);
            
            BindLoading(loadRequests, target, messageLoader, messageModelFactory)
                .DisposeWith(_serviceDisposable);

            var visibleRangeChanges = Observable.FromEventPattern<VisibleRangeNotifiedArgs>(
                h => trigger.VisibleRangeNotified += h,
                h => trigger.VisibleRangeNotified -= h)
                .Select(args => args.EventArgs);
            
            BindAvatarLoading(visibleRangeChanges, avatarLoader)
                .DisposeWith(_serviceDisposable);

            BindMediaLoading(visibleRangeChanges)
                .DisposeWith(_serviceDisposable);
        }

        private IDisposable BindAvatarLoading(
            IObservable<VisibleRangeNotifiedArgs> visibleRangeChanges,
            IAvatarLoader avatarLoader)
        {
            var prevRange = new VisibleRangeNotifiedArgs(0, 0);
            
            return visibleRangeChanges
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ObserveOn(TaskPoolScheduler.Default)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(range =>
                {
                    // release prev avatar bitmaps
                    for (int i = prevRange.From; i <= prevRange.To; i++)
                    {
                        int index = i;
                        
                        // do not release items within current range
                        if (index >= range.From && index <= range.To)
                        {
                            continue;
                        }

                        _items.Edit(list =>
                        {
                            var item = list[index];
                            if (item is MessageModel messageModel)
                            {
                                messageModel.Avatar.Bitmap = null;
                            }
                        });
                    }
                    
                    // load avatar bitmaps for new range
                    for (int i = range.From; i <= range.To; i++)
                    {
                        int index = i;
                        
                        _items.Edit(list =>
                        {
                            var item = list[index];
                            if (item is MessageModel messageModel)
                            {
                                var user = messageModel.Message.User;
                                var chat = messageModel.Message.Chat;
                                
                                if (messageModel.Avatar?.Bitmap == null)
                                {
                                    messageModel.Avatar = user == null
                                        ? avatarLoader.GetAvatar(chat, AvatarSize.Big)
                                        : avatarLoader.GetAvatar(user, AvatarSize.Big);
                                }
                                
                                if (messageModel.Avatar?.Bitmap == null)
                                {
                                    if (user == null)
                                    {
                                        avatarLoader.LoadAvatar(chat, AvatarSize.Big)
                                            .ObserveOn(TaskPoolScheduler.Default)
                                            .ObserveOn(RxApp.MainThreadScheduler)
                                            .Subscribe(avatar =>
                                            {
                                                messageModel.Avatar = avatar; 
                                            });
                                    }
                                    else
                                    {
                                        avatarLoader.LoadAvatar(user, AvatarSize.Big)
                                            .ObserveOn(TaskPoolScheduler.Default)
                                            .ObserveOn(RxApp.MainThreadScheduler)
                                            .Subscribe(avatar =>
                                            {
                                                messageModel.Avatar = avatar; 
                                            });
                                    }
                                }
                            }
                        });
                    }
                    
                    prevRange = range;
                });
        }

        private IDisposable BindMediaLoading(IObservable<VisibleRangeNotifiedArgs> visibleRangeChanges)
        {
            return Disposable.Empty; // TODO:
        }

        private IDisposable BindLoading(
            IObservable<MessageLoadRequestedArgs> loadRequests,
            Target target,
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory)
        {
            switch (target)
            {
                case Chat chat:
                    return BindChatLoading(
                        loadRequests,
                        chat,
                        messageLoader,
                        messageModelFactory);
                
                case Aggregate aggregate:
                    return BindAggregateLoading(
                        loadRequests,
                        aggregate,
                        messageLoader,
                        messageModelFactory);
            }

            return Disposable.Empty;
        }

        private IDisposable BindChatLoading(
            IObservable<MessageLoadRequestedArgs> loadRequests,
            Chat chat,
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory)
        {
            return loadRequests
                .ObserveOn(TaskPoolScheduler.Default)
                .SelectMany(signal =>
                {
                    var messages = signal.Direction == LoadDirection.Next
                        ? LoadNextMessages(chat, messageLoader) 
                        : LoadPrevMessages(chat, messageLoader);

                    return messages.Aggregate(new List<Message>(), (list, m) =>
                        {
                            list.Add(m);
                            return list;
                        })
                        .Select(list => new
                        {
                            Direction = signal.Direction,
                            Messages = list
                        });
                })
                .Subscribe(data =>
                {
                    var direction = data.Direction;
                    var models = data.Messages
                        .Select(messageModelFactory.CreateMessage)
                        .Reverse()
                        .ToList();
                    
                    switch (direction)
                    {
                        case LoadDirection.Prev:
                            _items.InsertRange(models, 0);
                            break;
                        
                        case LoadDirection.Next:
                            _items.AddRange(models);
                            break;
                    }
                });
        }

        private IDisposable BindAggregateLoading(
            IObservable<MessageLoadRequestedArgs> loadRequests,
            Aggregate aggregate,
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory)
        {
            throw new NotImplementedException();
        }

        private IObservable<Message> LoadNextMessages(Chat chat, IMessageLoader messageLoader)
        {
            var fromMessageId = chat.ChatData.LastReadInboxMessageId;
            
            if (_items.Count > 0)
            {
                var messageModel = (MessageModel)_items.Items.FirstOrDefault(i => i is MessageModel);
                if (messageModel != null)
                {
                    fromMessageId = messageModel.Message.MessageData.Id;
                }
            }
            
            return messageLoader.LoadNextMessages(chat, fromMessageId, 20);
        }

        private IObservable<Message> LoadPrevMessages(Chat chat, IMessageLoader messageLoader)
        {
            var fromMessageId = chat.ChatData.LastReadInboxMessageId;
            
            if (_items.Count > 0)
            {
                var messageModel = (MessageModel)_items.Items.LastOrDefault(i => i is MessageModel);
                if (messageModel != null)
                {
                    fromMessageId = messageModel.Message.MessageData.Id;
                }
            }
            
            return messageLoader.LoadPrevMessages(chat, fromMessageId, 20);
        }

        public void Dispose()
        {
            _serviceDisposable.Dispose();
        }
    }
}