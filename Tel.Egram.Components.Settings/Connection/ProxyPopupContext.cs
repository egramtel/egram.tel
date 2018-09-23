using Tel.Egram.Components.Popup;

namespace Tel.Egram.Components.Settings.Connection
{
    public class ProxyPopupContext : PopupContext
    {
        public ProxyPopupContext(IPopupController popupController)
            : base(PopupKind.Proxy, popupController)
        {
            PopupTitle = "Proxy settings";
        }
    }
}