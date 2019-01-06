using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Messenger.Explorer.Messages.Visual;

namespace Tel.Egram.Gui.Views.Messenger.Explorer.Messages.Visual
{
    public class StickerMessageControl : BaseControl<StickerMessageModel>
    {
        public StickerMessageControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
