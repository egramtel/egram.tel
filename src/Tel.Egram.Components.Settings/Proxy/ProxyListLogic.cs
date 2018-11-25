using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
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
                context.BindRemoveAction(proxyManager),
                context.BindEnableAction(proxyManager),
                context.BindAddAction(proxyManager),
                context.BindSaveAction(proxyManager),
                context.BindList(proxyManager));
        }

        private static IDisposable BindList(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {
            return proxyManager.GetAllProxies()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(proxies =>
                {
                    var disabledProxy = ProxyModel.DisabledProxy();
                    disabledProxy.EnableCommand = context.EnableProxyCommand;
                    
                    var otherProxies = proxies
                        .Select(p =>
                        {
                            var proxyModel = ProxyModel.FromProxy(p);
                            proxyModel.RemoveCommand = context.RemoveProxyCommand;
                            proxyModel.EnableCommand = context.EnableProxyCommand;
                            return proxyModel;
                        })
                        .ToList();
                    
                    context.Proxies = new ObservableCollectionExtended<ProxyModel>();
                    context.Proxies.Add(disabledProxy);
                    context.Proxies.AddRange(otherProxies);
                    
                    if (context.SelectedProxy == null)
                    {
                        context.SelectedProxy = otherProxies.FirstOrDefault(p => p.IsEnabled)
                            ?? disabledProxy;
                    }

                    if (!proxies.Any(p => p.IsEnabled))
                    {
                        disabledProxy.IsEnabled = true;
                    }
                });
        }

        private static IDisposable BindRemoveAction(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {
            context.RemoveProxyCommand = ReactiveCommand.CreateFromObservable(
                (ProxyModel proxyModel) => context.RemoveProxy(proxyManager, proxyModel),
                null,
                RxApp.MainThreadScheduler);

            return context.RemoveProxyCommand
                .Subscribe(proxyModel =>
                {
                    if (proxyModel == context.SelectedProxy)
                    {
                        context.SelectedProxy = context.Proxies.FirstOrDefault();
                    }
            
                    context.Proxies.Remove(proxyModel);
                });
        }

        private static IDisposable BindEnableAction(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {
            context.EnableProxyCommand = ReactiveCommand.CreateFromObservable(
                (ProxyModel proxyModel) => context.EnableProxy(proxyManager, proxyModel),
                null,
                RxApp.MainThreadScheduler);

            return context.EnableProxyCommand
                .Subscribe(proxyModel =>
                {
                    if (!proxyModel.IsEnabled)
                    {
                        foreach (var proxy in context.Proxies)
                        {
                            proxy.IsEnabled = false;
                        }
                
                        proxyModel.IsEnabled = true;
                    }
                });
        }

        private static IDisposable BindAddAction(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {
            context.AddProxyCommand = ReactiveCommand.CreateFromObservable(
                () => context.AddProxy(proxyManager),
                null,
                RxApp.MainThreadScheduler);

            return context.AddProxyCommand
                .Subscribe(proxyModel =>
                {
                    proxyModel.RemoveCommand = context.RemoveProxyCommand;
                    proxyModel.EnableCommand = context.EnableProxyCommand;
            
                    context.Proxies.Add(proxyModel);
                    context.SelectedProxy = proxyModel;
                });
        }

        private static IDisposable BindSaveAction(
            this ProxyPopupContext context,
            IProxyManager proxyManager)
        {   
            context.SaveProxyCommand = ReactiveCommand.CreateFromObservable(
                (ProxyModel proxyModel) => context.SaveProxy(proxyManager, proxyModel),
                null,
                RxApp.MainThreadScheduler);

            return context.SaveProxyCommand
                .Subscribe(proxyModel =>
                {
                    proxyModel.IsSaved = true;
                });
        }

        private static IObservable<ProxyModel> AddProxy(
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
            return Observable.Return(proxyModel);
        }

        private static IObservable<ProxyModel> RemoveProxy(
            this ProxyPopupContext context,
            IProxyManager proxyManager,
            ProxyModel proxyModel)
        {
            if (proxyModel.Proxy != null && proxyModel.Proxy.Id != 0)
            {
                return proxyManager.RemoveProxy(proxyModel.Proxy)
                    .Select(_ => proxyModel);
            }
            
            return Observable.Return(proxyModel);
        }

        private static IObservable<ProxyModel> SaveProxy(
            this ProxyPopupContext context,
            IProxyManager proxyManager,
            ProxyModel proxyModel)
        {
            if (proxyModel.Proxy.Id == 0)
            {
                return proxyManager
                    .AddProxy(proxyModel.ToProxy())
                    .Do(proxy => proxyModel.Proxy = proxy)
                    .Select(_ => proxyModel);
            }

            return proxyManager
                .UpdateProxy(proxyModel.Proxy)
                .Do(proxy => proxyModel.Proxy = proxy)
                .Select(_ => proxyModel);
        }

        private static IObservable<ProxyModel> EnableProxy(
            this ProxyPopupContext context,
            IProxyManager proxyManager,
            ProxyModel proxyModel)
        {
            if (!proxyModel.IsEnabled)
            {
                if (proxyModel.Proxy != null)
                {
                    return proxyManager
                        .EnableProxy(proxyModel.Proxy)
                        .Select(_ => proxyModel);
                }
                
                return proxyManager
                    .DisableProxy()
                    .Select(_ => proxyModel);
            }
            
            return Observable.Return(proxyModel);
        }
    }
}