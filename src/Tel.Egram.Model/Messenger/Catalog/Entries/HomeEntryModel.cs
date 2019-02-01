namespace Tel.Egram.Model.Messenger.Catalog.Entries
{
    public class HomeEntryModel : EntryModel
    {
        private HomeEntryModel()
        {
        }

        static HomeEntryModel()
        {
            Instance = new HomeEntryModel
            {
                Id = -1,
                Order = -1,
                Title = "Home"
            };
        }
        
        public static HomeEntryModel Instance { get; }
    }
}