using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Tel.Egram.Components.Application;

namespace Tel.Egram.Gui.Views.Application
{
    public class MainWindow : BaseWindow<MainWindowModel>
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools();
        }
    }
}
