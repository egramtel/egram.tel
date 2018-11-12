using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Authentication;

namespace Tel.Egram.Gui.Views.Authentication
{
    public class AuthenticationControl : ReactiveUserControl<AuthenticationModel>
    {
        public AuthenticationControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
