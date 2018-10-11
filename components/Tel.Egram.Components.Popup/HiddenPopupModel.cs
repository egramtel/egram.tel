namespace Tel.Egram.Components.Popup
{
    public class HiddenPopupModel : PopupModel
    {
        public HiddenPopupModel(IPopupController popupController)
            : base(PopupKind.Hidden, popupController)
        {
            IsPopupVisible = false;
        }
    }
}