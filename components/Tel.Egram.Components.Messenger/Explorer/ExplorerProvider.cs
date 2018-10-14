using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Tel.Egram.Components.Messenger.Explorer.Messages;
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
            IMessageModelFactory messageModelFactory
            )
        {
            _items = new SourceList<ItemModel>();
            
            BindLoading(target, trigger, messageLoader, messageModelFactory)
                .DisposeWith(_serviceDisposable);
        }

        private IDisposable BindLoading(
            Target target,
            IExplorerTrigger trigger,
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory)
        {
            switch (target)
            {
                case Chat chat:
                    return BindChatLoading(
                        chat,
                        trigger,
                        messageLoader,
                        messageModelFactory);
                
                case Aggregate aggregate:
                    return BindAggregateLoading(
                        aggregate,
                        trigger, 
                        messageLoader,
                        messageModelFactory);
            }

            return Disposable.Empty;
        }

        private IDisposable BindChatLoading(
            Chat chat,
            IExplorerTrigger trigger,
            IMessageLoader messageLoader, 
            IMessageModelFactory messageModelFactory)
        {
            return trigger
                .ObserveOn(TaskPoolScheduler.Default)
                .SubscribeOn(TaskPoolScheduler.Default)
                .SelectMany(kind =>
                {
                    var messages = kind is ExplorerSignal.LoadNext
                        ? LoadNextMessages(chat, messageLoader) 
                        : LoadPrevMessages(chat, messageLoader);

                    return messages.Aggregate(new List<Message>(), (list, m) =>
                        {
                            list.Add(m);
                            return list;
                        })
                        .Select(list => new
                        {
                            Kind = kind,
                            Messages = list
                        });
                })
                .Subscribe(data =>
                {
                    var kind = data.Kind;
                    var models = data.Messages
                        .Select(messageModelFactory.CreateMessage)
                        .Reverse()
                        .ToList();
                    
                    switch (kind)
                    {
                        case ExplorerSignal.LoadPrev _:
                            _items.InsertRange(models, 0);
                            break;
                        
                        case ExplorerSignal.LoadNext _:
                            _items.AddRange(models);
                            break;
                    }
                });
        }

        private IDisposable BindAggregateLoading(
            Aggregate aggregate,
            IExplorerTrigger trigger,
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