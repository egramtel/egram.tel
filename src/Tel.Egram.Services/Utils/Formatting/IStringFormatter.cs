using System;

namespace Tel.Egram.Services.Utils.Formatting
{
    public interface IStringFormatter
    {
        string FormatShortTime(DateTimeOffset dateTimeOffset);

        string FormatShortTime(int timestamp);

        string FormatMemorySize(long bytes);
    }
}