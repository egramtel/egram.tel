using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui.Views.Application
{
    public class StartupPage : UserControl
    {
        public StartupPage()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
