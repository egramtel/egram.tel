using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Messenger.Explorer.Messages;

namespace Tel.Egram.Gui.Views.Messenger.Explorer.Messages
{
    public class PhotoMessageControl : BaseControl<PhotoMessageModel>
    {
        public PhotoMessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
