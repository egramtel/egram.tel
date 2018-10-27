using System.Reactive;
using DynamicData.Binding;
using ReactiveUI;
using Tel.Egram.Models.Application.Popup;

namespace Tel.Egram.Models.Settings.Connection
{
    public class ProxyPopupModel : PopupModel
    {
        public ReactiveCommand<Unit, Unit> AddProxyCommand { get; set; }
        
        public ReactiveCommand<Unit, Unit> RemoveProxyCommand { get; set; }
        
        public ReactiveCommand<Unit, Unit> SaveProxyCommand { get; set; }
        
        public bool IsProxyEnabled { get; set; }
        
        public ProxyModel SelectedProxy { get; set; }
        
        public ObservableCollectionExtended<ProxyModel> Proxies { get; set; }
    }
}