using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using TdLib;
using Tel.Egram.Settings;

namespace Tel.Egram.Components.Settings.Proxy
{
    public static class ProxyListLogic
    {
        public static IDisposable BindProxyLogic(
            this ProxyPopupContext context)
        {
            return BindProxyLogic(
                context,
                Locator.Current.GetService<IProxyManager>());
        }
        
        public static IDisposable BindProxyLogic(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {
            return new CompositeDisposable(
                context.BindList(proxyManager),
                context.BindActions(proxyManager));
        }

        private static IDisposable BindList(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {
            return proxyManager.GetAllProxies()
                .Select(ProxyModel.FromProxy)
                .StartWith(ProxyModel.DisabledProxy())
                .Subscribe(proxy =>
                {
                    context.Proxies.Add(proxy);
                    
                    if (context.SelectedProxy == null)
                    {
                        context.SelectedProxy = context.Proxies.FirstOrDefault();
                    }
                });
        }

        private static IDisposable BindActions(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {
            context.AddProxyCommand = ReactiveCommand.CreateFromObservable(
                () => context.AddProxy(proxyManager), null, RxApp.MainThreadScheduler);

            return Disposable.Empty;
        }

        private static IObservable<TdApi.Ok> AddProxy(
            this ProxyPopupContext context,
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
            proxyModel.RemoveCommand = ReactiveCommand.CreateFromObservable(
                () => context.RemoveProxy(proxyManager, proxyModel), null, RxApp.MainThreadScheduler);
            
            context.Proxies.Add(proxyModel);
            context.SelectedProxy = proxyModel;

            return Observable.Empty<TdApi.Ok>();
        }

        private static IObservable<TdApi.Ok> RemoveProxy(
            this ProxyPopupContext context,
            IProxyManager proxyManager,
            ProxyModel proxyModel)
        {
            if (proxyModel == context.SelectedProxy)
            {
                context.SelectedProxy = context.Proxies.FirstOrDefault();
            }
            
            context.Proxies.Remove(proxyModel);
            
            if (proxyModel.Proxy != null)
            {
                return proxyManager.RemoveProxy(proxyModel.Proxy);
            }
            
            return Observable.Empty<TdApi.Ok>();
        }

        private static IObservable<TdApi.Proxy> SaveProxy(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {
            var proxyModel = context.SelectedProxy;

            if (proxyModel != null)
            {
                if (proxyModel.Proxy.Id != 0)
                {
                    return proxyManager
                        .AddProxy(proxyModel.ToProxy())
                        .Do(proxy => proxyModel.Proxy = proxy);
                }
 
                return proxyManager
                    .UpdateProxy(proxyModel.Proxy)
                    .Do(proxy => proxyModel.Proxy = proxy);
            }

            return Observable.Empty<TdApi.Proxy>();
        }
    }
}