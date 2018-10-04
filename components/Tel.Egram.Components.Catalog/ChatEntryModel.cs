using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Catalog
{
    public class ChatEntryModel : EntryModel
    {
        public Chat Chat { get; set; }
        
        public static ChatEntryModel FromChat(Chat chat)
        {
            var title = chat.ChatData.Title;
            
            return new ChatEntryModel
            {
                Id = chat.ChatData.Id,
                Chat = chat,
                Title = title
            };
        }
    }
}