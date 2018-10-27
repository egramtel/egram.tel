using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Tel.Egram.Gui.Views.Messenger.Catalog
{
    [DoNotNotify]
    public class CatalogControl : UserControl
    {
        public CatalogControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
