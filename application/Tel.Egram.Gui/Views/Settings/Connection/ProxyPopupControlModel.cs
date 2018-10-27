using System.Reactive;
using DynamicData.Binding;
using ReactiveUI;
using Tel.Egram.Components.Settings.Connection;
using Tel.Egram.Gui.Views.Application;

namespace Tel.Egram.Gui.Views.Settings
{
    public class ProxyPopupControlModel : PopupControlModel
    {
        public ReactiveCommand<Unit, Unit> AddProxyCommand { get; set; }
        
        public ReactiveCommand<Unit, Unit> RemoveProxyCommand { get; set; }
        
        public ReactiveCommand<Unit, Unit> SaveProxyCommand { get; set; }
        
        public bool IsProxyEnabled { get; set; }
        
        public ProxyModel SelectedProxy { get; set; }
        
        public ObservableCollectionExtended<ProxyModel> Proxies { get; set; }
    }
}