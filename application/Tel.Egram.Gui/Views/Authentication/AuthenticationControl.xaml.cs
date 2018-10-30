using Avalonia;
using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Authentication;
using ReactiveUI;

namespace Tel.Egram.Gui.Views.Authentication
{
    public class AuthenticationControl : ReactiveUserControl<AuthenticationController>
    {
        public AuthenticationControl()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
