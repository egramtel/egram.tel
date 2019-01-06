using System;
using System.Reactive.Disposables;
using ReactiveUI;
using Splat;
using Tel.Egram.Model.Popups;
using Tel.Egram.Model.Settings.Proxy;

namespace Tel.Egram.Model.Authentication
{
    public static class ProxySettingsLogic
    {
        public static IDisposable BindProxySettings(
            this AuthenticationModel model)
        {
            return BindProxySettings(
                model,
                Locator.Current.GetService<IPopupController>());
        }
        
        public static IDisposable BindProxySettings(
            this AuthenticationModel model,
            IPopupController popupController)
        {
            model.SetProxyCommand = ReactiveCommand.Create(
                () => popupController.Show(new ProxyPopupContext()),
                null,
                RxApp.MainThreadScheduler);
            
            return Disposable.Empty;
        }
    }
}