using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using Tel.Egram.Components.Messenger.Explorer.Messages;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Messaging.Messages;
using Tel.Egram.Utils.Reactive;

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
            Chat chat, 
            Message fromMessage)
        {
            return GetPrevMessages(chat, fromMessage)
                .Select(_messageModelFactory.CreateMessage)
                .CollectToList()
                .Select(list =>
                {
                    list.Reverse();
                    return list;
                });
        }

        public IObservable<IList<MessageModel>> LoadPrevMessages(
            Chat chat)
        {
            return LoadPrevMessages(chat, null);
        }

        public IObservable<IList<MessageModel>> LoadNextMessages(
            Chat chat, 
            Message fromMessage)
        {
            return GetNextMessages(chat, fromMessage)
                .Select(_messageModelFactory.CreateMessage)
                .CollectToList()
                .Select(list =>
                {
                    list.Reverse();
                    return list;
                });
        }

        public IObservable<IList<MessageModel>> LoadNextMessages(
            Chat chat)
        {
            return LoadNextMessages(chat, null);
        }

        private IObservable<Message> GetNextMessages(
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

        private IObservable<Message> GetPrevMessages(
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