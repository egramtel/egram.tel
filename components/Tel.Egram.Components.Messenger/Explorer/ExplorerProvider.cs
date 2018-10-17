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
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Utils;

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
            IAvatarManager avatarManager
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
            
            BindAvatarLoading(visibleRangeChanges, avatarManager)
                .DisposeWith(_serviceDisposable);

            BindMediaLoading(visibleRangeChanges)
                .DisposeWith(_serviceDisposable);
        }

        private IDisposable BindAvatarLoading(
            IObservable<VisibleRangeNotifiedArgs> visibleRangeChanges,
            IAvatarManager avatarManager)
        {
            var prevRange = new Range(0, 0);
            
            return visibleRangeChanges
                .ObserveOn(TaskPoolScheduler.Default)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(args =>
                {
                    var range = args.Range;
                    
                    avatarManager.ReleaseAvatars(_items, prevRange, range)
                        .DisposeWith(_serviceDisposable);
                    
                    avatarManager.LoadAvatars(_items, prevRange, range)
                        .DisposeWith(_serviceDisposable);
                    
                    prevRange = range;
                });
        }

        private IDisposable BindMediaLoading(
            IObservable<VisibleRangeNotifiedArgs> visibleRangeChanges)
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