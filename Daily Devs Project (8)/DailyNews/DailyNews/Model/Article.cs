namespace DailyNews.Model
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }         
        public string Url { get; set; }           
        public string Content { get; set; }       
        public DateTime PublishedAt { get; set; } 
        public int RssSourceId { get; set; }

        public Article(string title, string url, string content, DateTime publishedAt, int rssSourceId)
        {
            Title = title;
            Url = url;
            Content = content;
            PublishedAt = publishedAt;
            RssSourceId = rssSourceId;
        }

        public Article() { }
    }
}
