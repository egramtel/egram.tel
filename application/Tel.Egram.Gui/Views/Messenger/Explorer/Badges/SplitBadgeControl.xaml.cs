using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Messenger.Explorer.Badges
{
    [DoNotNotify]
    public class SplitBadgeControl : UserControl
    {
        public SplitBadgeControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
