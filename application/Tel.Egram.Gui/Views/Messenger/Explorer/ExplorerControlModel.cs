using DynamicData.Binding;
using PropertyChanged;
using Tel.Egram.Gui.Views.Messenger.Explorer.Items;
using Tel.Egram.Messaging.Chats;
using Tel.Egram.Utils;

namespace Tel.Egram.Gui.Views.Messenger.Explorer
{
    [AddINotifyPropertyChangedInterface]
    public class ExplorerControlModel
    {
        public bool IsVisible { get; set; }
        
        public ObservableCollectionExtended<ItemModel> Items { get; set; }
        
        public Range VisibleRange { get; set; }
        
        public Target Target { get; set; }
        
        public static ExplorerControlModel Hidden()
        {
            return new ExplorerControlModel();
        }

        public static ExplorerControlModel FromTarget(Target target)
        {
            return new ExplorerControlModel
            {
                IsVisible = true,
                Target = target,
                Items = new ObservableCollectionExtended<ItemModel>()
            };
        }
    }
}