using System;
using System.Reactive.Disposables;
using ReactiveUI;
using Splat;
using Tel.Egram.Components.Popup;

namespace Tel.Egram.Components.Authentication
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
                () => popupController.Show(new PopupContext()),
                null,
                RxApp.MainThreadScheduler);
            
            return Disposable.Empty;
        }
    }
}