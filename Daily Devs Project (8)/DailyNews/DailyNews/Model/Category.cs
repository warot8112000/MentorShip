using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<RSS_Category> RssCategories { get; set; }
        public ICollection<Article_Category> ArticleCategory { get; set; }

    }
}
