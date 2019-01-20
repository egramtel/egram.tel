using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Settings.Proxy;

namespace Tel.Egram.Views.Popups.Proxy
{
    public class ProxyItemControl : BaseControl<ProxyModel>
    {   
        public ProxyItemControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
