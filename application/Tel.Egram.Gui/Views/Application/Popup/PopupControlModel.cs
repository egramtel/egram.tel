using System.Reactive;
using ReactiveUI;

namespace Tel.Egram.Gui.Views.Application
{
    public class PopupControlModel
    {
        public string PopupTitle { get; set; }
        
        public bool IsPopupVisible { get; set; }
        
        public int PopupIndex { get; set; }
        
        public ReactiveCommand<Unit, Unit> PopupCloseCommand { get; }
    }
}