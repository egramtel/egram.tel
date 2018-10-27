using Tel.Egram.Components.Settings.Connection;
using Tel.Egram.Gui.Views.Settings;
using Tel.Egram.Gui.Views.Settings.Connection;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Settings
{
    public class SettingsController
        : BaseController<SettingsControlModel>, ISettingsController
    {
        private readonly IFactory<ProxyPopupControlModel, IProxyPopupController> _proxyPopupControllerFactory;
        private IProxyPopupController _proxyPopupController;

        public SettingsController(IFactory<ProxyPopupControlModel, IProxyPopupController> proxyPopupControllerFactory)
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