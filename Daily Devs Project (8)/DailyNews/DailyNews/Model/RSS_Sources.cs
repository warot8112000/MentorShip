using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DailyNews.Model
{
    [Table("RSS_Sources")]
    public class RSS_Sources
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        [JsonIgnore] //bỏ tuần tự hóa từ Rss_Category -> StringJson
        public ICollection<RSS_Category> RssCategories { get; set; }
    }
}
