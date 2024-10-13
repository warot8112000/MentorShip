using System.ComponentModel.DataAnnotations;

namespace DailyNews.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
    }
}
