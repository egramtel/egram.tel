using System.Reactive;
using System.Reactive.Disposables;
using DynamicData.Binding;
using ReactiveUI;
using TdLib;
using Tel.Egram.Components.Popup;

namespace Tel.Egram.Components.Settings.Proxy
{
    public class ProxyPopupContext : PopupContext, ISupportsActivation
    {
        public ReactiveCommand<Unit, TdApi.Ok> AddProxyCommand { get; set; }
        
        public bool IsProxyEnabled { get; set; }
        
        public ProxyModel SelectedProxy { get; set; }
        
        public ObservableCollectionExtended<ProxyModel> Proxies { get; set; }
            = new ObservableCollectionExtended<ProxyModel>();

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public ProxyPopupContext()
        {
            this.WhenActivated(disposables =>
            {
                Title = "Proxy configuration";
                
                this.BindProxyLogic()
                    .DisposeWith(disposables);
            });
        }
    }
}