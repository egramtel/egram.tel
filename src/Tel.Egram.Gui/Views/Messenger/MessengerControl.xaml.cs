using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tel.Egram.Gui.Views.Messenger
{
    public class MessengerControl : UserControl
    {
        public MessengerControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
