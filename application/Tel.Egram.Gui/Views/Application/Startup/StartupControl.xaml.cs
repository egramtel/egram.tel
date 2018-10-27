using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui.Views.Application.Startup
{
    public class StartupControl : UserControl
    {
        public StartupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
