using System;
using Tel.Egram.Gui.Views.Application;

namespace Tel.Egram.Components.Application
{
    public class ApplicationPopupController : IApplicationPopupController
    {
        public event EventHandler<PopupControlModel> ContextChanged;
        
        public void HidePopup()
        {
            ContextChanged?.Invoke(this, null);
        }

        public void ShowPopup(PopupControlModel popupControlModel)
        {
            ContextChanged?.Invoke(this, popupControlModel);
        }
    }
}