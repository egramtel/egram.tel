using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using TdLib;
using Tel.Egram.Components.Popup;
using Tel.Egram.Settings;

namespace Tel.Egram.Components.Settings.Connection
{
    public class ProxyPopupModel : PopupModel
    {
        private readonly CompositeDisposable _modelDisposable = new CompositeDisposable();
        
        public ReactiveCommand<Unit, Unit> AddProxyCommand { get; }
        
        public ReactiveCommand<Unit, Unit> RemoveProxyCommand { get; }
        
        public ReactiveCommand<Unit, Unit> SaveProxyCommand { get; }
        
        public bool IsProxyEnabled { get; set; }
        
        public ProxyModel SelectedProxy { get; set; }
        
        public ReactiveList<ProxyModel> Proxies { get; set; }
        
        public ProxyPopupModel(
            IPopupController popupController,
            IProxyManager proxyManager
            )
            : base(PopupKind.Proxy, popupController)
        {
            PopupTitle = "Proxy settings";

            this.WhenAnyValue(c => c.IsProxyEnabled)
                .Subscribe(isEnabled => ToggleProxy(proxyManager, isEnabled))
                .DisposeWith(_modelDisposable);
            
            AddProxyCommand = ReactiveCommand.CreateFromObservable(
                () => AddProxy(proxyManager));
            
            RemoveProxyCommand = ReactiveCommand.CreateFromObservable(
                () => RemoveProxy(proxyManager));
            
            SaveProxyCommand = ReactiveCommand.CreateFromObservable(
                () => SaveProxy(proxyManager));
            
            LoadProxies(proxyManager);
        }

        private void ToggleProxy(IProxyManager proxyManager, bool isEnabled)
        {
            if (isEnabled)
            {
                if (SelectedProxy != null)
                {
                    proxyManager.DisableProxy()
                        .Concat(proxyManager.EnableProxy(SelectedProxy.Proxy))
                        .Subscribe()
                        .DisposeWith(_modelDisposable);
                }
            }
            else
            {
                proxyManager.DisableProxy()
                    .Subscribe()
                    .DisposeWith(_modelDisposable);
            }
        }

        private void LoadProxies(IProxyManager proxyManager)
        {
            proxyManager.GetAllProxies()
                .SubscribeOn(TaskPoolScheduler.Default)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(proxies =>
                {
                    var models = proxies.Select(ProxyModel.FromProxy);
                    Proxies = new ReactiveList<ProxyModel>(models);
                })
                .DisposeWith(_modelDisposable);
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
            
            var model = ProxyModel.FromProxy(proxy);
            Proxies.Add(model);
            
            return Observable.Empty<Unit>();
        }

        private IObservable<Unit> RemoveProxy(IProxyManager proxyManager)
        {
            var model = SelectedProxy;

            if (model != null)
            {
                Proxies.Remove(model);
                
                if (model.Proxy.Id != 0)
                {
                    return proxyManager.RemoveProxy(model.Proxy);
                }
            }
            
            return Observable.Empty<Unit>();
        }

        private IObservable<Unit> SaveProxy(IProxyManager proxyManager)
        {
            var model = SelectedProxy;

            if (model != null)
            {
                if (model.Proxy.Id != 0)
                {
                    return proxyManager.AddProxy(model.ToProxy())
                        .Do(proxy => model.Proxy = proxy)
                        .Select(_ => Unit.Default);
                }
                
                return proxyManager.UpdateProxy(model.Proxy)
                    .Do(proxy => model.Proxy = proxy)
                    .Select(_ => Unit.Default);
            }

            return Observable.Empty<Unit>();
        }

        public override void Dispose()
        {
            base.Dispose();
            _modelDisposable.Dispose();
        }
    }
}