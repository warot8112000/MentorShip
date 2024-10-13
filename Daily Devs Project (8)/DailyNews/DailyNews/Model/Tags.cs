using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("Tags")]
    public class Tags
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User_Tags> UserTags { get; set; }
    }
}
