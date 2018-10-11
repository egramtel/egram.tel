using System;
using System.Reactive;
using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Components.Popup
{
    [AddINotifyPropertyChangedInterface]
    public abstract class PopupModel : IDisposable
    {
        public string PopupTitle { get; set; }
        
        public bool IsPopupVisible { get; set; }
        
        public int PopupIndex { get; set; }
        
        public ReactiveCommand<Unit, Unit> PopupCloseCommand { get; }

        public PopupModel(
            PopupKind popupKind,
            IPopupController popupController
            )
        {
            PopupTitle = "";
            IsPopupVisible = true;
            PopupIndex = (int)popupKind;
            
            PopupCloseCommand = ReactiveCommand.Create(popupController.Hide);
        }

        public virtual void Dispose()
        {
        }
    }
}