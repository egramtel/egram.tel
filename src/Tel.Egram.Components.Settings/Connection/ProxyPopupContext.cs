using System.Reactive;
using DynamicData.Binding;
using ReactiveUI;
using Tel.Egram.Components.Popup;

namespace Tel.Egram.Components.Settings.Connection
{
    public class ProxyPopupContext : PopupContext, ISupportsActivation
    {
        public ReactiveCommand<Unit, Unit> AddProxyCommand { get; set; }
        
        public ReactiveCommand<Unit, Unit> RemoveProxyCommand { get; set; }
        
        public ReactiveCommand<Unit, Unit> SaveProxyCommand { get; set; }
        
        public bool IsProxyEnabled { get; set; }
        
        public ProxyModel SelectedProxy { get; set; }
        
        public ObservableCollectionExtended<ProxyModel> Proxies { get; set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}