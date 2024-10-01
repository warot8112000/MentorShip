namespace DailyNews.Model
{
    public class RssSource
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Url { get; set; } 
        public string? Description { get; set; }
    }
}
