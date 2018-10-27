using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using TdLib;
using Tel.Egram.Gui.Views.Settings;
using Tel.Egram.Settings;

namespace Tel.Egram.Components.Settings.Connection
{
    public class ProxyPopupController
        : BaseController, IProxyPopupController
    {
        public ProxyPopupController(
            ProxyPopupControlModel model,
            IProxyManager proxyManager)
        {
            BindProxies(model, proxyManager)
                .DisposeWith(this);

            BindActions(model, proxyManager)
                .DisposeWith(this);
        }

        private IDisposable BindProxies(
            ProxyPopupControlModel model,
            IProxyManager proxyManager)
        {
            return proxyManager.GetAllProxies()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(proxies =>
                {
                    var models = proxies.Select(ProxyModel.FromProxy);
                    model.Proxies = new ObservableCollectionExtended<ProxyModel>(models);
                });
        }

        private IDisposable BindActions(
            ProxyPopupControlModel model,
            IProxyManager proxyManager)
        {
            model.PopupTitle = "Proxy settings";

            model.AddProxyCommand = ReactiveCommand.CreateFromObservable(
                () => AddProxy(model, proxyManager));
            
            model.RemoveProxyCommand = ReactiveCommand.CreateFromObservable(
                () => RemoveProxy(model, proxyManager));
            
            model.SaveProxyCommand = ReactiveCommand.CreateFromObservable(
                () => SaveProxy(model, proxyManager));
            
            return model.WhenAnyValue(c => c.IsProxyEnabled)
                .Select(isEnabled => ToggleProxy(model, proxyManager, isEnabled))
                .Subscribe();
        }

        private IObservable<Unit> ToggleProxy(
            ProxyPopupControlModel model,
            IProxyManager proxyManager,
            bool isEnabled)
        {
            if (isEnabled)
            {
                if (model.SelectedProxy != null)
                {
                    return proxyManager.DisableProxy()
                        .Concat(proxyManager.EnableProxy(model.SelectedProxy.Proxy));
                }
            }
            else
            {
                proxyManager.DisableProxy();
            }

            return Observable.Empty<Unit>();
        }

        private IObservable<Unit> AddProxy(
            ProxyPopupControlModel model,
            IProxyManager proxyManager)
        {
            var proxy = new TdApi.Proxy
            {
                Server = null,
                Port = 0,
                Type = new TdApi.ProxyType.ProxyTypeSocks5
                {
                    Username = null,
                    Password = null
                }
            };
            
            var proxyModel = ProxyModel.FromProxy(proxy);
            model.Proxies.Add(proxyModel);
            
            return Observable.Empty<Unit>();
        }

        private IObservable<Unit> RemoveProxy(
            ProxyPopupControlModel model,
            IProxyManager proxyManager)
        {
            var proxyModel = model.SelectedProxy;

            if (proxyModel != null)
            {
                model.Proxies.Remove(proxyModel);
                
                if (proxyModel.Proxy.Id != 0)
                {
                    return proxyManager.RemoveProxy(proxyModel.Proxy);
                }
            }
            
            return Observable.Empty<Unit>();
        }

        private IObservable<Unit> SaveProxy(
            ProxyPopupControlModel model,
            IProxyManager proxyManager)
        {
            var proxyModel = model.SelectedProxy;

            if (proxyModel != null)
            {
                if (proxyModel.Proxy.Id != 0)
                {
                    return proxyManager.AddProxy(proxyModel.ToProxy())
                        .Do(proxy => proxyModel.Proxy = proxy)
                        .Select(_ => Unit.Default);
                }
                
                return proxyManager.UpdateProxy(proxyModel.Proxy)
                    .Do(proxy => proxyModel.Proxy = proxy)
                    .Select(_ => Unit.Default);
            }

            return Observable.Empty<Unit>();
        }
    }
}