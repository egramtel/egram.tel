using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Messenger.Explorer.Messages.Basic;

namespace Tel.Egram.Views.Messenger.Explorer.Messages.Basic
{
    public class TextMessageControl : BaseControl<TextMessageModel>
    {
        public TextMessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
