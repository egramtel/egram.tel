using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Popups;

namespace Tel.Egram.Gui.Views.Popups
{
    public class PopupControl : BaseControl<PopupModel>
    {
        public PopupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
