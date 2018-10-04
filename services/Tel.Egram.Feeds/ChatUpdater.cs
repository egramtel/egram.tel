using System;
using System.Reactive;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.TdLib;

namespace Tel.Egram.Feeds
{
    public class ChatUpdater : IChatUpdater
    {
        private readonly TdAgent _agent;

        public ChatUpdater(TdAgent agent)
        {
            _agent = agent;
        }

        public IObservable<Unit> GetOrderChanges()
        {
            var orderUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatOrder>()
                .Select(_ => Unit.Default);
            var messageUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatLastMessage>()
                .Select(_ => Unit.Default);
            var pinnedUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatIsPinned>()
                .Select(_ => Unit.Default);
            var draftUpdates = _agent.Updates.OfType<TdApi.Update.UpdateChatDraftMessage>()
                .Select(_ => Unit.Default);

            return orderUpdates
                .Merge(messageUpdates)
                .Merge(pinnedUpdates)
                .Merge(draftUpdates);
        }
    }
}