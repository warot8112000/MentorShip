using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("RSS_Category")]
    public class RSS_Category
    {
        public int Id { get; set; }
        public int RssSourceId { get; set; }
        public int CategoryId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public RSS_Sources RssSource { get; set; }
        public Category Category { get; set; }
        public ICollection<Articles> Articles { get; set; }
    }
}
