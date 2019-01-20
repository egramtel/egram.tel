using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Messenger;

namespace Tel.Egram.Views.Messenger
{
    public class MessengerControl : BaseControl<MessengerModel>
    {
        public MessengerControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
