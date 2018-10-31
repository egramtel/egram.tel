using PropertyChanged;
using Tel.Egram.Graphics;
using Tel.Egram.Messaging.Chats;

namespace Tel.Egram.Models.Messenger.Catalog.Entries
{
    [AddINotifyPropertyChangedInterface]
    public class EntryModel
    {
        public long Id { get; set; }
        
        public int Order { get; set; }
        
        public string Title { get; set; }
        
        public Avatar Avatar { get; set; }

        public bool HasUnread { get; set; }

        public string UnreadCount { get; set; }
        
        public Target Target { get; set; }

        public static EntryModel FromTarget(Target target)
        {
            return new EntryModel
            {
                Target = target
            };
        }
    }
}