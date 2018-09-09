using Avalonia.Media;
using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Catalog
{
    public class ChatEntryModel : EntryModel
    {
        public Chat Chat { get; set; }
        
        public static ChatEntryModel FromChat(Chat chat)
        {
            var title = chat.Ch.Title;
            var init = string.IsNullOrEmpty(title) ? null : title.Substring(0, 1).ToUpper();
            
            return new ChatEntryModel
            {
                Chat = chat,
                Title = title,
                Init = init
            };
        }
    }
}