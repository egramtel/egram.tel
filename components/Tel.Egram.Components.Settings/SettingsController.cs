using Tel.Egram.Components.Settings.Connection;
using Tel.Egram.Gui.Views.Settings;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Settings
{
    public class SettingsController
        : BaseController, ISettingsController
    {
        private readonly IFactory<ProxyPopupControlModel, IProxyPopupController> _proxyPopupControllerFactory;
        private IProxyPopupController _proxyPopupController;

        public SettingsController(
            SettingsControlModel model,
            IFactory<ProxyPopupControlModel, IProxyPopupController> proxyPopupControllerFactory)
        {
            _proxyPopupControllerFactory = proxyPopupControllerFactory;
        }

        public override void Dispose()
        {
            _proxyPopupController?.Dispose();
            
            base.Dispose();
        }
    }
}