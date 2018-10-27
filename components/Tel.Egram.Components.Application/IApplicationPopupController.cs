using System;
using Tel.Egram.Gui.Views.Application;
using Tel.Egram.Gui.Views.Application.Popup;

namespace Tel.Egram.Components.Application
{
    public interface IApplicationPopupController : IPopupController
    {
        event EventHandler<PopupControlModel> ContextChanged;
    }
}