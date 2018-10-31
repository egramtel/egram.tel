using Tel.Egram.Models.Application.Popup;

namespace Tel.Egram.Components
{
    public interface IPopupController
    {
        void HidePopup();

        void ShowPopup(PopupModel popupModel);
    }
}