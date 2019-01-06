using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Messenger.Informer;

namespace Tel.Egram.Gui.Views.Messenger.Informer
{
    public class InformerControl : BaseControl<InformerModel>
    {
        public InformerControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
