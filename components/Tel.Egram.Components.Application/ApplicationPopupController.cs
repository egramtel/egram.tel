using System;
using Tel.Egram.Components.Popup;

namespace Tel.Egram.Components.Application
{
    public class ApplicationPopupController : IApplicationPopupController
    {
        public event EventHandler<PopupContext> ContextChanged;
        
        public void Hide()
        {
            ContextChanged?.Invoke(this, null);
        }

        public void Show(PopupContext popupContext)
        {
            ContextChanged?.Invoke(this, popupContext);
        }
    }
}