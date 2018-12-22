using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Messenger.Explorer.Messages;

namespace Tel.Egram.Gui.Views.Messenger.Explorer.Messages
{
    public class VideoMessageControl : BaseControl<VideoMessageModel>
    {
        public VideoMessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
