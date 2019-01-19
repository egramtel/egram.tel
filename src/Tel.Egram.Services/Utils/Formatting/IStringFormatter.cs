using System;

namespace Tel.Egram.Services.Utils.Formatting
{
    public interface IStringFormatter
    {
        string AsShortTime(DateTimeOffset dateTimeOffset);

        string AsShortTime(int timestamp);
    }
}