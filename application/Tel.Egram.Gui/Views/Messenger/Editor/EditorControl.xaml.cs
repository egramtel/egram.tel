using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Messenger.Editor
{
    [DoNotNotify]
    public class EditorControl : UserControl
    {
        public EditorControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
