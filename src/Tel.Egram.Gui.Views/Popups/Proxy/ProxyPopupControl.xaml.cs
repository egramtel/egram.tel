using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Settings.Proxy;

namespace Tel.Egram.Gui.Views.Popups.Proxy
{
    public class ProxyPopupControl : BaseControl<ProxyPopupContext>
    {   
        public ProxyPopupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
