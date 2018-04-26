using ReactiveUI;

namespace Egram.Components.Chatting
{
    public class ChannelAuthor : MessageAuthor
    {
        private long _chatId;
        public long ChatId
        {
            get => _chatId;
            set => this.RaiseAndSetIfChanged(ref _chatId, value);
        }
    }
}