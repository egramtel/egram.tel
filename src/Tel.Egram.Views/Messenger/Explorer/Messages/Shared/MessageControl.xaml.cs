using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace Tel.Egram.Views.Messenger.Explorer.Messages.Shared
{
    public class MessageControl : ContentControl
    {
        public MessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
