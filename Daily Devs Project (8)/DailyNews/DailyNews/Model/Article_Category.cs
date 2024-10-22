using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("Article_Category")]
    public class Article_Category
    {
        public int ArticleId { get; set; }
        public Articles Article { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
