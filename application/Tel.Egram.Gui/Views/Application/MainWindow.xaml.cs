using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Application
{
    [DoNotNotify]
    public class MainWindow : Window
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools();
        }
    }
}
