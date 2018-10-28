using DynamicData.Binding;
using PropertyChanged;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Models.Messenger.Explorer.Items;
using Tel.Egram.Utils;

namespace Tel.Egram.Models.Messenger.Explorer
{
    [AddINotifyPropertyChangedInterface]
    public class ExplorerModel
    {
        public bool IsVisible { get; set; } = true;
        
        public ObservableCollectionExtended<ItemModel> Items { get; set; }
            = new ObservableCollectionExtended<ItemModel>();
        
        public Range VisibleRange { get; set; }
        
        public static ExplorerModel Hidden()
        {
            return new ExplorerModel
            {
                IsVisible = false
            };
        }
    }
}