using System;
using Tel.Egram.Components.Popup;

namespace Tel.Egram.Components.Application
{
    public interface IApplicationPopupController : IPopupController
    {
        event EventHandler<PopupModel> ContextChanged;
    }
}