using System;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Messenger.Explorer.Triggers
{
    public interface IExplorerTrigger
    {
        event EventHandler<MessageLoadRequestedArgs> MessageLoadRequested;

        event EventHandler<VisibleRangeNotifiedArgs> VisibleRangeNotified;

        void LoadMessages(LoadDirection direction);

        void NotifyVisibleRange(Range range);
    }
}