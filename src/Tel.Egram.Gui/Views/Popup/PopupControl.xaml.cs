using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Popup;

namespace Tel.Egram.Gui.Views.Popup
{
    public class PopupControl : BaseControl<PopupModel>
    {
        public PopupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
