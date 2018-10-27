using System;
using Tel.Egram.Gui.Views.Application;

namespace Tel.Egram.Components.Application
{
    public interface IApplicationPopupController : IPopupController
    {
        event EventHandler<PopupControlModel> ContextChanged;
    }
}