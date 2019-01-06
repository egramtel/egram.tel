using System.Reactive;
using System.Reactive.Disposables;
using DynamicData.Binding;
using ReactiveUI;
using Tel.Egram.Model.Popups;

namespace Tel.Egram.Model.Settings.Proxy
{
    public class ProxyPopupContext : PopupContext, ISupportsActivation
    {
        public ReactiveCommand<Unit, ProxyModel> AddProxyCommand { get; set; }
        
        public ReactiveCommand<ProxyModel, ProxyModel> SaveProxyCommand { get; set; }
        
        public ReactiveCommand<ProxyModel, ProxyModel> EnableProxyCommand { get; set; }
        
        public ReactiveCommand<ProxyModel, ProxyModel> RemoveProxyCommand { get; set; }
        
        public bool IsProxyEnabled { get; set; }
        
        public ProxyModel SelectedProxy { get; set; }
        
        public ObservableCollectionExtended<ProxyModel> Proxies { get; set; }

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