using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui.Views.Messenger
{
    public class MessageControl : UserControl
    {
        public MessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
