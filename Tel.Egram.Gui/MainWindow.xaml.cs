using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools();
        }
    }
}
