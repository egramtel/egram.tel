using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Authentication
{
    [DoNotNotify]
    public class AuthenticationPage : UserControl
    {
        public AuthenticationPage()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
