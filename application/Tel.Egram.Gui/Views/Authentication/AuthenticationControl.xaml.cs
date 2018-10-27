using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Authentication
{
    [DoNotNotify]
    public class AuthenticationControl : UserControl
    {
        public AuthenticationControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
