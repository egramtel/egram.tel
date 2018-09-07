using System.Collections.Generic;
using System.Linq;
using Tel.Egram.Feeds;
using Tel.Egram.Messages;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Chats
{
    public class ChatMessengerContext : MessengerContext
    {
        private readonly ChatFeed _feed;
        private readonly MessageAvatarInteractor _messageAvatarInteractor;

        public ChatMessengerContext(
            ChatFeed feed,
            IFactory<ChatMessengerContext, MessageAvatarInteractor> messageAvatarInteractorFactory
            )
        {
            _feed = feed;
            _messageAvatarInteractor = messageAvatarInteractorFactory.Create(this);
        }

        public override void OnPrependMessages(List<Message> messages)
        {
            var messageModels = messages.Select(MessageModel.FromMessage)
                .ToList();
        }

        public override void OnAppendMessages(List<Message> messages)
        {
            var messageModels = messages.Select(MessageModel.FromMessage)
                .Reverse()
                .ToList();

            foreach (var messageModel in messageModels)
            {
                // get cached avatar
                if (messageModel.AuthorAvatar == null)
                {
                    // load avatar
                    _messageAvatarInteractor.LoadAuthorAvatar(messageModel);
                }
            }
            
            Messages.InsertRange(0, messageModels);
        }

        public override void Dispose()
        {
            
        }
    }
}