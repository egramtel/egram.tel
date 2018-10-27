using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Messenger
{
    [DoNotNotify]
    public class MessengerControl : UserControl
    {
        public MessengerControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
