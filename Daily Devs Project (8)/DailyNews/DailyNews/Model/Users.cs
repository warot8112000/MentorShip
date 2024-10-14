using System.ComponentModel.DataAnnotations.Schema;

namespace DailyNews.Model
{
    [Table("Users")]

    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string PasswordHash { get; set; }
        public ICollection<User_Tags> UserTags { get; set; }
    }
}
