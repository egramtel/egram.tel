namespace Tel.Egram.Components.Popup
{
    public class HiddenPopupContext : PopupContext
    {
        public HiddenPopupContext(IPopupController popupController)
            : base(PopupKind.Hidden, popupController)
        {
            IsPopupVisible = false;
        }
    }
}