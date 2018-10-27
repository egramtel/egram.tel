using Tel.Egram.Gui.Views.Application;
using Tel.Egram.Gui.Views.Application.Popup;

namespace Tel.Egram.Components
{
    public interface IPopupController
    {
        void HidePopup();

        void ShowPopup(PopupControlModel popupControlModel);
    }
}