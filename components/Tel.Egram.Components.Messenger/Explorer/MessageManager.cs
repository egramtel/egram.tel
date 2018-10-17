using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public class MessageManager : IMessageManager
    {
        private readonly IMessageLoader _messageLoader;
        private readonly IMessageModelFactory _messageModelFactory;

        public MessageManager(
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory)
        {
            _messageLoader = messageLoader;
            _messageModelFactory = messageModelFactory;
        }
        
        public IDisposable LoadPrevMessages(
            Target target, 
            SourceList<ItemModel> items)
        {
            switch (target)
            {
                case Chat chat:
                    return LoadPrevMessages(chat, items)
                        .Aggregate(new List<Message>(), (list, m) =>
                        {
                            list.Add(m);
                            return list;
                        })
                        .Subscribe(messages =>
                        {
                            var models = messages
                                .Select(_messageModelFactory.CreateMessage)
                                .Reverse()
                                .ToList();
                            
                            items.InsertRange(models, 0);
                        });
            }
            
            return Disposable.Empty;
        }

        public IDisposable LoadNextMessages(
            Target target, 
            SourceList<ItemModel> items)
        {
            switch (target)
            {
                case Chat chat:
                    return LoadNextMessages(chat, items)
                        .Aggregate(new List<Message>(), (list, m) =>
                        {
                            list.Add(m);
                            return list;
                        })
                        .Subscribe(messages =>
                        {
                            var models = messages
                                .Select(_messageModelFactory.CreateMessage)
                                .Reverse()
                                .ToList();
                            
                            items.AddRange(models);
                        });
            }
            
            return Disposable.Empty;
        }
        
        private IObservable<Message> LoadNextMessages(
            Chat chat,
            SourceList<ItemModel> items)
        {
            var fromMessageId = chat.ChatData.LastReadInboxMessageId;
            
            if (items.Count > 0)
            {
                var messageModel = (MessageModel)items.Items.FirstOrDefault(i => i is MessageModel);
                if (messageModel != null)
                {
                    fromMessageId = messageModel.Message.MessageData.Id;
                }
            }
            
            return _messageLoader.LoadNextMessages(chat, fromMessageId, 20);
        }

        private IObservable<Message> LoadPrevMessages(
            Chat chat,
            SourceList<ItemModel> items)
        {
            var fromMessageId = chat.ChatData.LastReadInboxMessageId;
            
            if (items.Count > 0)
            {
                var messageModel = (MessageModel)items.Items.LastOrDefault(i => i is MessageModel);
                if (messageModel != null)
                {
                    fromMessageId = messageModel.Message.MessageData.Id;
                }
            }
            
            return _messageLoader.LoadPrevMessages(chat, fromMessageId, 20);
        }
    }
}