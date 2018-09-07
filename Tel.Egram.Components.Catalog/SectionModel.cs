using ReactiveUI;

namespace Tel.Egram.Components.Catalog
{
    public class SectionModel : ReactiveObject
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private ReactiveList<EntryModel> _entries = new ReactiveList<EntryModel>();
        public ReactiveList<EntryModel> Entries
        {
            get => _entries;
            set => this.RaiseAndSetIfChanged(ref _entries, value);
        }
    }
}