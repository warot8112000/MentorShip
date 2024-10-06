using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("RSS_Sources")]
    public class RSS_Sources
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public ICollection<RSS_Category> RssCategories { get; set; }
    }
}
