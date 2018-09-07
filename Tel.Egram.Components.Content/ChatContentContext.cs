using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Tel.Egram.Components.Catalog;
using Tel.Egram.Components.Messenger;
using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Content
{
    public class ChatContentContext : ContentContext
    {
        private int _contentIndex;
        public int ContentIndex
        {
            get => _contentIndex;
            set => this.RaiseAndSetIfChanged(ref _contentIndex, value);
        }
        
        private MessengerContext _messengerContext;
        public MessengerContext MessengerContext
        {
            get => _messengerContext;
            set => this.RaiseAndSetIfChanged(ref _messengerContext, value);
        }
        
        public ChatContentContext(ContentKind kind) : base(kind)
        {
        }

        public override void Dispose()
        {
            
        }
    }
}