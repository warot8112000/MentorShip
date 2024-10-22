using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("Likes")]
    public class Likes
    {
        public int UserId { get; set; } 
        public int ArticleId { get; set; } 
        public DateTime LikedAt { get; set; } = DateTime.Now; 

        public Users User { get; set; } 
        public Articles Article { get; set; }
    }
}
