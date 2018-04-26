using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Egram.Gui.Chatting
{
    public class MessageControl : UserControl
    {
        public MessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
