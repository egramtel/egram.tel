namespace Tel.Egram.Components.Popup
{
    public interface IPopupController
    {
        void Hide();

        void Show(PopupContext popupContext);
    }
}