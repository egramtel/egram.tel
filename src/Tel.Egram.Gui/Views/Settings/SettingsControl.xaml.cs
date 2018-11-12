using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Settings;

namespace Tel.Egram.Gui.Views.Settings
{
    public class SettingsControl : ReactiveUserControl<SettingsModel>
    {
        public SettingsControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
