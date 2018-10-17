using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer.Triggers
{
    public class VisibleRangeNotifiedArgs
    {
        public Range Range { get; }
            
        public VisibleRangeNotifiedArgs(Range range)
        {
            Range = range;
        }
    }
}