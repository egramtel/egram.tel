using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui
{
    public class LoadingPage : UserControl
    {
        public LoadingPage()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
