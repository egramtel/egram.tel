using Tel.Egram.Components.Settings.Connection;
using Tel.Egram.Models.Settings;
using Tel.Egram.Models.Settings.Connection;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Settings
{
    public class SettingsController
        : BaseController<SettingsModel>, ISettingsController
    {
        private readonly IFactory<ProxyPopupModel, IProxyPopupController> _proxyPopupControllerFactory;
        private IProxyPopupController _proxyPopupController;

        public SettingsController(IFactory<ProxyPopupModel, IProxyPopupController> proxyPopupControllerFactory)
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