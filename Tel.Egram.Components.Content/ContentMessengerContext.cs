using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Tel.Egram.Components.Catalog;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Feeds;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Content
{
    public class ContentMessengerContext : ContentContext
    {
        private readonly Target _target;
        private readonly IFactory<Aggregate, AggregateMessengerContext> _aggregateMessengerContextFactory;
        private readonly IFactory<Chat, ChatMessengerContext> _chatMessengerContextFactory;

        private MessengerContext _messengerContext;
        public MessengerContext MessengerContext
        {
            get => _messengerContext;
            set => this.RaiseAndSetIfChanged(ref _messengerContext, value);
        }
        
        public ContentMessengerContext(
            Target target,
            IFactory<Aggregate, AggregateMessengerContext> aggregateMessengerContextFactory,
            IFactory<Chat, ChatMessengerContext> chatMessengerContextFactory
            )
            : base(ContentKind.Chat)
        {
            _target = target;
            _aggregateMessengerContextFactory = aggregateMessengerContextFactory;
            _chatMessengerContextFactory = chatMessengerContextFactory;

            switch (_target)
            {
                case Aggregate aggregate:
                    MessengerContext = _aggregateMessengerContextFactory.Create(aggregate);
                    break;
                
                case Chat chat:
                    MessengerContext = _chatMessengerContextFactory.Create(chat);
                    break;
            }
        }

        public override void Dispose()
        {
            MessengerContext?.Dispose();
        }
    }
}