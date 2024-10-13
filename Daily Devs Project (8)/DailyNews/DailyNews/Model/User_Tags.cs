using Azure;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("User_Tags")]
    public class User_Tags
    {
        public int UserId { get; set; }
        public Users User { get; set; }

        public int TagId { get; set; }
        public Tags Tag { get; set; }
    }
}
