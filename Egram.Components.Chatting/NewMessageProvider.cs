using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using Egram.Components.TDLib;

namespace Egram.Components.Chatting
{
    public class NewMessageProvider
    {
        private readonly IAgent _agent;
        private readonly MessageMapper _messageMapper;

        public NewMessageProvider(
            IAgent agent,
            MessageMapper messageMapper
            )
        {
            _agent = agent;
            _messageMapper = messageMapper;
        }

        public IObservable<BaseMessage> Appends(TD.Chat chat)
        {
            return Observable.FromEventPattern<TD.Structure>(
                    h => _agent.Received += h,
                    h => _agent.Received -= h)
                .Select(s => s.EventArgs as TD.Update.UpdateNewMessage)
                .Where(u => u != null && u.Message.ChatId == chat.Id)
                .Select(MapMessage);
        }

        private BaseMessage MapMessage(TD.Update.UpdateNewMessage update)
        {
            return _messageMapper.Map(update.Message, null);
        }
    }
}
