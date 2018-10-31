using System;
using System.Reactive;
using TdLib;

namespace Tel.Egram.Messaging.Messages
{
    public interface IMessageSender
    {
        IObservable<TdApi.Message> SendMessage(
            TdApi.Chat chat,
            TdApi.InputMessageContent.InputMessageText messageTextContent);
    }
}