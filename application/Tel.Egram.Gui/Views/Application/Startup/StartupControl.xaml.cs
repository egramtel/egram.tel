using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Application.Startup
{
    [DoNotNotify]
    public class StartupControl : UserControl
    {
        public StartupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
