using System;
using Tel.Egram.Components.Popup;

namespace Tel.Egram.Components.Application
{
    public class ApplicationPopupController : IApplicationPopupController
    {
        public event EventHandler<PopupModel> ContextChanged;
        
        public void Hide()
        {
            ContextChanged?.Invoke(this, null);
        }

        public void Show(PopupModel popupModel)
        {
            ContextChanged?.Invoke(this, popupModel);
        }
    }
}