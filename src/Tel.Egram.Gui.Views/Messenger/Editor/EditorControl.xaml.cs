using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Messenger.Editor;

namespace Tel.Egram.Gui.Views.Messenger.Editor
{
    public class EditorControl : BaseControl<EditorModel>
    {
        public EditorControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
