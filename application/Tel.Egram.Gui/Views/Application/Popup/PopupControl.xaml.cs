using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Application.Popup
{
    [DoNotNotify]
    public class PopupControl : UserControl
    {
        public PopupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
