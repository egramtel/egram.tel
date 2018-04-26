using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Egram.Gui.Chatting
{
    public class ChatControl : UserControl
    {
        public ChatControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
