using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _accountService.LoginAsync(loginDto);

            if (user == null)
            {
                return Unauthorized(new { message = "Thông tin đăng nhập không hợp lệ" });
            }

            return Ok(new { message = "Đăng nhập thành công", userId = user.Id });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = await _accountService.RegisterAsync(register);
            if (newUser == null)
            {
                return BadRequest(new { message = "Đăng ký thất bại" });
            }

            return Ok(new { message = "Đăng ký thành công", userId = newUser.Id });
        }
    }
}
