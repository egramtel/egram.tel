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
using Tel.Egram.Models.Settings.Connection;
using Tel.Egram.Settings;
using Tel.Egram.Utils;

namespace Tel.Egram.Components.Settings.Connection
{
    public class ProxyPopupController
        : Controller<ProxyPopupModel>, IProxyPopupController
    {
        public ProxyPopupController(
            ISchedulers schedulers,
            IProxyManager proxyManager)
        {
            BindProxies(schedulers, proxyManager).DisposeWith(this);
            BindActions(schedulers, proxyManager).DisposeWith(this);
        }

        private IDisposable BindProxies(
            ISchedulers schedulers,
            IProxyManager proxyManager)
        {
            return proxyManager.GetAllProxies()
                .SubscribeOn(schedulers.Pool)
                .ObserveOn(schedulers.Main)
                .Subscribe(proxies =>
                {
                    var models = proxies.Select(ProxyModel.FromProxy);
                    Model.Proxies = new ObservableCollectionExtended<ProxyModel>(models);
                });
        }

        private IDisposable BindActions(
            ISchedulers schedulers,
            IProxyManager proxyManager)
        {
            Model.PopupTitle = "Proxy settings";

            Model.AddProxyCommand = ReactiveCommand.CreateFromObservable(
                () => AddProxy(proxyManager));
            
            Model.RemoveProxyCommand = ReactiveCommand.CreateFromObservable(
                () => RemoveProxy(proxyManager));
            
            Model.SaveProxyCommand = ReactiveCommand.CreateFromObservable(
                () => SaveProxy(proxyManager));
            
            return Model.WhenAnyValue(c => c.IsProxyEnabled)
                .Select(isEnabled => ToggleProxy(proxyManager, isEnabled))
                .Subscribe();
        }

        private IObservable<Unit> ToggleProxy(
            IProxyManager proxyManager,
            bool isEnabled)
        {
            if (isEnabled)
            {
                if (Model.SelectedProxy != null)
                {
                    return proxyManager.DisableProxy()
                        .Concat(proxyManager.EnableProxy(Model.SelectedProxy.Proxy));
                }
            }
            else
            {
                proxyManager.DisableProxy();
            }

            return Observable.Empty<Unit>();
        }

        private IObservable<Unit> AddProxy(IProxyManager proxyManager)
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
            Model.Proxies.Add(proxyModel);
            
            return Observable.Empty<Unit>();
        }

        private IObservable<Unit> RemoveProxy(IProxyManager proxyManager)
        {
            var proxyModel = Model.SelectedProxy;

            if (proxyModel != null)
            {
                Model.Proxies.Remove(proxyModel);
                
                if (proxyModel.Proxy.Id != 0)
                {
                    return proxyManager.RemoveProxy(proxyModel.Proxy);
                }
            }
            
            return Observable.Empty<Unit>();
        }

        private IObservable<Unit> SaveProxy(IProxyManager proxyManager)
        {
            var proxyModel = Model.SelectedProxy;

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