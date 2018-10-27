using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Messenger.Explorer.Messages
{
    [DoNotNotify]
    public class TextMessageControl : UserControl
    {
        public TextMessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
