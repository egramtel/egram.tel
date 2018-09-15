using Tel.Egram.Components.Messenger;
using Tel.Egram.Feeds;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Content
{
    public class ContentMessengerContext : ContentContext
    {
        private readonly IFactory<Aggregate, AggregateMessengerContext> _aggregateMessengerContextFactory;
        private readonly IFactory<Chat, ChatMessengerContext> _chatMessengerContextFactory;
        private readonly Target _target;

        public MessengerContext MessengerContext { get; set; }
        
        public ContentMessengerContext(
            IFactory<Aggregate, AggregateMessengerContext> aggregateMessengerContextFactory,
            IFactory<Chat, ChatMessengerContext> chatMessengerContextFactory,
            Target target)
            : base(ContentKind.Chat)
        {
            _aggregateMessengerContextFactory = aggregateMessengerContextFactory;
            _chatMessengerContextFactory = chatMessengerContextFactory;
            _target = target;

            MessengerContext?.Dispose();
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

        public override void Dispose() => MessengerContext?.Dispose();
    }
}