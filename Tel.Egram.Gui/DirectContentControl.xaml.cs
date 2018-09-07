using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui
{
    public class DirectContentControl : UserControl
    {
        public DirectContentControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
