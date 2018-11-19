using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Settings.Proxy;

namespace Tel.Egram.Gui.Views.Popup.Proxy
{
    public class ProxyItemControl : BaseControl<ProxyModel>
    {   
        public ProxyItemControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
