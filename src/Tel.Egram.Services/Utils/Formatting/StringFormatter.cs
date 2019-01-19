using System;

namespace Tel.Egram.Services.Utils.Formatting
{
    public class StringFormatter : IStringFormatter
    {
        public string AsShortTime(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("hh:mm");
        }

        public string AsShortTime(int timestamp)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(timestamp)
                .ToLocalTime();

            return AsShortTime(time);
        }
    }
}