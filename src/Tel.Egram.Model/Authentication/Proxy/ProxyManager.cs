using System;
using ReactiveUI;
using Splat;
using Tel.Egram.Model.Popups;
using Tel.Egram.Model.Settings.Proxy;

namespace Tel.Egram.Model.Authentication.Proxy
{
    public class ProxyManager
    {
        private readonly IPopupController _popupController;

        public ProxyManager(
            IPopupController popupController)
        {
            _popupController = popupController;
        }

        public ProxyManager()
            : this (
                Locator.Current.GetService<IPopupController>())
        {
        }
        
        public IDisposable Bind(AuthenticationModel model)
        {
            model.SetProxyCommand = ReactiveCommand.Create(
                () => _popupController.Show(new ProxyPopupContext()),
                null,
                RxApp.MainThreadScheduler);

            return model.SetProxyCommand;
        }
    }
}