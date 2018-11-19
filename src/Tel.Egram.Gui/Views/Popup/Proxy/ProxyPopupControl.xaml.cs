using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Settings.Proxy;

namespace Tel.Egram.Gui.Views.Popup.Proxy
{
    public class ProxyPopupControl : BaseControl<ProxyPopupContext>
    {   
        public ProxyPopupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
