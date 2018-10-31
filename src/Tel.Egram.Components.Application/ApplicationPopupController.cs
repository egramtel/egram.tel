using System;
using Tel.Egram.Models.Application.Popup;

namespace Tel.Egram.Components.Application
{
    public class ApplicationPopupController : IApplicationPopupController
    {
        public event EventHandler<PopupModel> ContextChanged;
        
        public void HidePopup()
        {
            ContextChanged?.Invoke(this, null);
        }

        public void ShowPopup(PopupModel popupModel)
        {
            ContextChanged?.Invoke(this, popupModel);
        }
    }
}