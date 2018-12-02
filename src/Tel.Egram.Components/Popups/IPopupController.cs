namespace Tel.Egram.Components.Popups
{
    public interface IPopupController
    {
        void Show(PopupContext context);
        void Hide();
    }
}