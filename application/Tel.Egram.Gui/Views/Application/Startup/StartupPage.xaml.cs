using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Application.Startup
{
    [DoNotNotify]
    public class StartupPage : UserControl
    {
        public StartupPage()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
