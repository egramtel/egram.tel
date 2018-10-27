using Tel.Egram.Gui.Views.Application;

namespace Tel.Egram.Components
{
    public interface IPopupController
    {
        void HidePopup();

        void ShowPopup(PopupControlModel popupControlModel);
    }
}