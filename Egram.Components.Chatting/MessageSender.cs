using System;
using System.Reactive;
using System.Reactive.Linq;
using Egram.Components.TDLib;

namespace Egram.Components.Chatting
{
    public class MessageSender
    {
        private readonly IAgent _agent;

        public MessageSender(IAgent agent)
        {
            _agent = agent;
        }
        
        public IObservable<TD.Message> SendText(TD.Chat chat, TD.InputMessageContent.InputMessageText messageText)
        {
            return Observable.FromAsync(ct => _agent.ExecuteAsync(new TD.SendMessage
            {
                ChatId = chat.Id,
                InputMessageContent = messageText
            }));
        }
    }
}