using Azure;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("Article_Tags")]
    public class Article_Tags
    {
        public int ArticleId { get; set; }
        public Articles Article { get; set; }

        public int TagId { get; set; }
        public Tags Tag { get; set; }
    }
}
