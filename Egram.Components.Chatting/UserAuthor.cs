using ReactiveUI;

namespace Egram.Components.Chatting
{
    public class UserAuthor : MessageAuthor
    {
        private int _userId;
        public int UserId
        {
            get => _userId;
            set => this.RaiseAndSetIfChanged(ref _userId, value);
        }
    }
}