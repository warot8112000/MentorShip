using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RssFeedService _rssService;
        private readonly IMapper _mapper;

        public AccountController(ApplicationDbContext context, RssFeedService rssService, IMapper mapper)
        {
            _context = context;
            _rssService = rssService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.PasswordHash == loginDto.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Thông tin đăng nhập không hợp lệ" });
            }

            return Ok(new { message = "Đăng nhập thành công", userId = user.Id });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == register.Username);
            if(user != null)
            {
                return Unauthorized(new { message = "Đăng ký thất bại" });
            }

            var newUser = new Users();
            newUser.Username = register.Username;
            newUser.PasswordHash = register.Password;
            newUser.Email = register.Email;

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công", userId = newUser.Id });
        }
    }
}
