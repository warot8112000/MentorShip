using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("Comments")]
    public class Comments
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int ArticleId { get; set; } 
        public string Content { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        public Users User { get; set; } 
        public Articles Article { get; set; }
    }
}
