namespace DailyNews.Model
{
    public class RSSCategory
    {
        public int Id { get; set; }
        public int RssSourceId { get; set; }
        public int CategoryId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public RSSSource RssSource { get; set; }
        public Category Category { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
