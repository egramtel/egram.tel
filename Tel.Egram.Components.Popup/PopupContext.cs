using System;
using System.Reactive;
using PropertyChanged;
using ReactiveUI;

namespace Tel.Egram.Components.Popup
{
    [AddINotifyPropertyChangedInterface]
    public abstract class PopupContext : IDisposable
    {
        public string PopupTitle { get; set; }

        public string PopupOkText { get; set; }
        
        public bool IsPopupVisible { get; set; }
        
        public int PopupIndex { get; set; }
        
        public ReactiveCommand<Unit, Unit> PopupOkCommand { get; }
        
        public ReactiveCommand<Unit, Unit> PopupCloseCommand { get; }

        public PopupContext(
            PopupKind popupKind,
            IPopupController popupController
            )
        {
            PopupTitle = "";
            PopupOkText = "OK";
            IsPopupVisible = true;
            PopupIndex = (int)popupKind;
            
            PopupOkCommand = ReactiveCommand.Create(popupController.Hide);
            PopupCloseCommand = ReactiveCommand.Create(popupController.Hide);
        }

        public virtual void Dispose()
        {
        }
    }
}