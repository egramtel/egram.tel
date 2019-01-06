namespace Tel.Egram.Model.Popups
{
    public interface IPopupController
    {
        void Show(PopupContext context);
        void Hide();
    }
}