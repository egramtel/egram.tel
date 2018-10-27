using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Messenger.Informer
{
    [DoNotNotify]
    public class InformerControl : UserControl
    {
        public InformerControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
