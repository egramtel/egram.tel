using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Settings
{
    [DoNotNotify]
    public class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
