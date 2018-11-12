using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Components.Messenger.Editor;

namespace Tel.Egram.Gui.Views.Messenger.Editor
{
    public class EditorControl : ReactiveUserControl<EditorModel>
    {
        public EditorControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
