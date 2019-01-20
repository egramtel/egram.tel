using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Views.Startup
{
    public class StartupControl : UserControl
    {
        public StartupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
