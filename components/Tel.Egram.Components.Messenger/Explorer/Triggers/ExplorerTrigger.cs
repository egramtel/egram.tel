using System;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer.Triggers
{
    public class ExplorerTrigger : IExplorerTrigger
    {
        public ExplorerTrigger()
        {
            
        }
        
        public event EventHandler<MessageLoadRequestedArgs> MessageLoadRequested;
        
        public event EventHandler<VisibleRangeNotifiedArgs> VisibleRangeNotified;
        
        public void LoadMessages(LoadDirection direction)
        {
            MessageLoadRequested?.Invoke(this, new MessageLoadRequestedArgs(direction));
        }

        public void NotifyVisibleRange(Range range)
        {
            VisibleRangeNotified?.Invoke(this, new VisibleRangeNotifiedArgs(range));
        }
    }
}
