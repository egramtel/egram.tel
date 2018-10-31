using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Models.Messenger.Explorer.Messages;

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
        
        public IObservable<IList<MessageModel>> LoadPrevMessages(
            Target target, 
            Message fromMessage)
        {
            switch (target)
            {
                case Chat chat:
                    return LoadPrevMessages(chat, fromMessage)
                        .Select(_messageModelFactory.CreateMessage)
                        .Aggregate(new List<MessageModel>(), (list, m) =>
                        {
                            list.Add(m);
                            return list;
                        })
                        .Select(list =>
                        {
                            list.Reverse();
                            return list;
                        });
            }
            
            return Observable.Return(new List<MessageModel>());
        }

        public IObservable<IList<MessageModel>> LoadPrevMessages(Target target)
        {
            return LoadPrevMessages(target, null);
        }

        public IObservable<IList<MessageModel>> LoadNextMessages(
            Target target, 
            Message fromMessage)
        {
            switch (target)
            {
                case Chat chat:
                    return LoadNextMessages(chat, fromMessage)
                        .Select(_messageModelFactory.CreateMessage)
                        .Aggregate(new List<MessageModel>(), (list, m) =>
                        {
                            list.Add(m);
                            return list;
                        })
                        .Select(list =>
                        {
                            list.Reverse();
                            return list;
                        });
            }
            
            return Observable.Return(new List<MessageModel>());
        }

        public IObservable<IList<MessageModel>> LoadNextMessages(Target target)
        {
            return LoadNextMessages(target, null);
        }

        private IObservable<Message> LoadNextMessages(
            Chat chat,
            Message fromMessage = null)
        {
            var fromMessageId = chat.ChatData.LastReadInboxMessageId;
            
            if (fromMessage != null)
            {
                fromMessageId = fromMessage.MessageData.Id;
            }
            
            return _messageLoader.LoadNextMessages(chat, fromMessageId, 32);
        }

        private IObservable<Message> LoadPrevMessages(
            Chat chat,
            Message fromMessage = null)
        {
            var fromMessageId = chat.ChatData.LastReadInboxMessageId;
            
            if (fromMessage != null)
            {
                fromMessageId = fromMessage.MessageData.Id;
            }
            
            return _messageLoader.LoadPrevMessages(chat, fromMessageId, 32);
        }
    }
}