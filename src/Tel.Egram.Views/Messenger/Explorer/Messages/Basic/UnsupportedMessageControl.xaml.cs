using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Messenger.Explorer.Messages;

namespace Tel.Egram.Views.Messenger.Explorer.Messages.Basic
{
    public class UnsupportedMessageControl : BaseControl<UnsupportedMessageModel>
    {
        public UnsupportedMessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
