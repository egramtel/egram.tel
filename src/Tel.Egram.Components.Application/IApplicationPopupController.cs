using System;
using Tel.Egram.Models.Application.Popup;

namespace Tel.Egram.Components.Application
{
    public interface IApplicationPopupController : IPopupController
    {
        event EventHandler<PopupModel> ContextChanged;
    }
}