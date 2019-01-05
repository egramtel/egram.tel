using ReactiveUI;
using Tel.Egram.Messaging.Messages;

namespace Tel.Egram.Components.Messenger.Explorer.Messages
{
    public class ReplyModel : ISupportsActivation
    {
        public string AuthorName { get; set; }
        
        public string Text { get; set; }
        
        public Message Message { get; set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}