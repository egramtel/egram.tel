namespace Tel.Egram.Components.Messenger.Explorer.Triggers
{
    public class VisibleRangeNotifiedArgs
    {
        public int From { get; }
        public int To { get; }
            
        public VisibleRangeNotifiedArgs(int from, int to)
        {
            From = from;
            To = to;
        }
    }
}