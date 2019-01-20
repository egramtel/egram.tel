using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Tel.Egram.Model.Messenger.Catalog.Entries;

namespace Tel.Egram.Views.Messenger.Catalog
{
    public class EntryControl : BaseControl<EntryModel>
    {
        public EntryControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
