namespace Tel.Egram.Components.Messenger.Explorer.Triggers
{
    public class MessageLoadRequestedArgs
    {
        public LoadDirection Direction { get; }
            
        public MessageLoadRequestedArgs(LoadDirection direction)
        {
            Direction = direction;
        }
    }
}