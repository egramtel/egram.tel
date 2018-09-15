using TdLib;
using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Messenger
{
    public class ChatMessengerContext : MessengerContext
    {
        private readonly IChatService _chatService;
        private readonly Chat _chat;

        public ChatMessengerContext(
            IChatService chatService,
            Chat chat
            )
        {
            _chatService = chatService;
            _chat = chat;

            IsMessageEditorVisible = !IsChannel();
        }

        private bool IsChannel()
        {
            if (_chat.ChatData.Type is TdApi.ChatType.ChatTypeSupergroup supergroup)
            {
                return supergroup.IsChannel;
            }

            return false;
        }
    }
}