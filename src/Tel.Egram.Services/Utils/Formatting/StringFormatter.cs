using System;

namespace Tel.Egram.Services.Utils.Formatting
{
    public class StringFormatter : IStringFormatter
    {
        public string FormatShortTime(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("hh:mm");
        }

        public string FormatShortTime(int timestamp)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(timestamp)
                .ToLocalTime();

            return FormatShortTime(time);
        }

        public string FormatMemorySize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1) {
                order++;
                bytes = bytes / 1024;
            }
            return $"{bytes:0.##} {sizes[order]}";
        }
    }
}