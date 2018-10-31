using PropertyChanged;
using Tel.Egram.Models.Messenger.Explorer.Badges;
using Tel.Egram.Models.Messenger.Explorer.Messages;

namespace Tel.Egram.Models.Messenger.Explorer.Items
{
    [AddINotifyPropertyChangedInterface]
    public abstract class ItemModel
    {
        public int Type { get; set; }

        public ItemModel()
        {
            Type = (int) GetItemType();
        }

        private ItemType GetItemType()
        {
            switch (this)
            {
                case SplitBadgeModel _:
                    return ItemType.Split;
                
                case DateBadgeModel _:
                    return ItemType.Date;
                
                case TextMessageModel _:
                    return ItemType.Text;
            }

            return ItemType.Unsupported;
        }
    }
}