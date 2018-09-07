using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Feeds;
using Tel.Egram.Messages;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Aggregate
{
    public class AggregateMessengerContext : MessengerContext
    {
        private readonly AggregateFeed _feed;
        
        private readonly AggregateMessageInteractor _aggregateMessageInteractor;
        private readonly MessageAvatarInteractor _messageAvatarInteractor;
        
        private IDisposable _loadInitialMessagesSubscription;

        public AggregateMessengerContext(
            AggregateFeed feed,
            IFactory<AggregateMessageInteractor> messageInteractorFactory,
            IFactory<MessageAvatarInteractor> messageAvatarInteractorFactory
            )
        {
            _feed = feed;
            
            _aggregateMessageInteractor = messageInteractorFactory.Create();
            _messageAvatarInteractor = messageAvatarInteractorFactory.Create();
            
            _loadInitialMessagesSubscription = _aggregateMessageInteractor.LoadInitialMessages(this, _feed);
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
            _loadInitialMessagesSubscription.Dispose();
        }
    }
}