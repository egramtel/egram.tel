using Tel.Egram.Feeds;

namespace Tel.Egram.Components.Catalog
{
    public class AggregateEntryModel : EntryModel
    {
        public Aggregate Aggregate { get; set; }
        
        public static AggregateEntryModel Main()
        {
            // TODO: i18n
            var title = "All Channels";
            var init = title.Substring(0, 1).ToUpper();
            
            return new AggregateEntryModel
            {
                Aggregate = new Aggregate
                {
                    Id = 0
                },
                Title = title,
                Init = init
            };
        }
    }
}