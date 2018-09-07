using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui
{
    public class ContentHostControl : UserControl
    {
        public ContentHostControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
