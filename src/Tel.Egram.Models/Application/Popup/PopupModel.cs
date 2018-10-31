using System.Reactive;
using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Models.Application.Popup
{
    [AddINotifyPropertyChangedInterface]
    public class PopupModel
    {
        public string PopupTitle { get; set; }
        
        public bool IsPopupVisible { get; set; }
        
        public int PopupIndex { get; set; }
        
        public ReactiveCommand<Unit, Unit> PopupCloseCommand { get; }
    }
}