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
        private readonly IChatLoader _chatLoader;
        private readonly IMessageLoader _messageLoader;
        private readonly IMessageModelFactory _messageModelFactory;

        public MessageManager(
            IChatLoader chatLoader,
            IMessageLoader messageLoader,
            IMessageModelFactory messageModelFactory)
        {
            _chatLoader = chatLoader;
            _messageLoader = messageLoader;
            _messageModelFactory = messageModelFactory;
        }
        
        public IObservable<IList<MessageModel>> LoadPrevMessages(
            Chat chat, 
            Message fromMessage = null)
        {
            return GetChat(chat)
                .SelectSeq(c => GetPrevMessages(c, fromMessage)
                    .Select(_messageModelFactory.CreateMessage)
                    .ToList()
                    .Select(list => list.Reverse().ToList()));
        }

        public IObservable<IList<MessageModel>> LoadInitMessages(
            Chat chat,
            Message fromMessage = null)
        {
            return Observable.Concat(
                LoadPrevMessages(chat, fromMessage),
                LoadMessage(chat, fromMessage),
                LoadNextMessages(chat, fromMessage))
                    .ToList()
                    .Select(list =>
                    {
                        var prev = list[0];
                        var current = list[1];
                        var next = list[2];
                        
                        return prev.Concat(current).Concat(next).ToList();
                    });
        }

        public IObservable<IList<MessageModel>> LoadNextMessages(
            Chat chat,
            Message fromMessage)
        {
            return GetChat(chat)
                .SelectSeq(c => GetNextMessages(c, fromMessage)
                    .Select(_messageModelFactory.CreateMessage)
                    .ToList())
                    .Select(list => list.Reverse().Skip(1).ToList());
        }

        private IObservable<IList<MessageModel>> LoadMessage(
            Chat chat,
            Message fromMessage)
        {   
            var messageId = chat.ChatData.LastReadInboxMessageId;
            
            if (fromMessage != null)
            {
                messageId = fromMessage.MessageData.Id;
            }

            return _messageLoader
                .LoadMessage(chat.ChatData.Id, messageId)
                .Select(_messageModelFactory.CreateMessage)
                .ToList();
        }

        private IObservable<Chat> GetChat(Chat chat)
        {
            return _chatLoader.LoadChat(chat.ChatData.Id);
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