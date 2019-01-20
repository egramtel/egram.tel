using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Popups;

namespace Tel.Egram.Views.Popups
{
    public class PopupControl : BaseControl<PopupModel>
    {
        public PopupControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
