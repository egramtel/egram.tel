using System;

namespace Egram.Components.Chatting
{
    public class DateBadge : ChatEntity
    {
        public readonly DateTimeOffset Date;
        
        public DateBadge(DateTimeOffset date)
        {
            Date = date;
        }

        public string Text => Date.ToString("dd.MM.yyyy");

        public override bool IsDateBadge => true;

        public override bool IsTextMessage => false;
    }
}