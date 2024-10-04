namespace DailyNews.Model
{
    public class RSSSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public ICollection<RSSCategory> RssCategories { get; set; }
    }
}
