using System;
using System.Collections.Generic;
using System.Text;

namespace Egram.Components.Chatting
{
    public class MessageMapper
    {
        public BaseMessage Map(TD.Message message, MessageAuthor author)
        {
            switch (message.Content)
            {
                case TD.MessageContent.MessageText mt:
                    var text = MapText(message, mt);
                    text.MessageAuthor = author;
                    return text;
            }

            return null;
        }

        private TextMessage MapText(TD.Message message, TD.MessageContent.MessageText mt)
        {
            var textMessage = new TextMessage
            {
                Id = message.Id,
                DateTime = DateTimeOffset.FromUnixTimeSeconds(message.Date),
                IsOutgoing = message.IsOutgoing,
                IsIncoming = !message.IsOutgoing,
                Text = mt.Text.Text
            };

            return textMessage;
        }
    }
}
