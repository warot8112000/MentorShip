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
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RssFeedService _rssService;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext context, RssFeedService rssService, IMapper mapper)
        {
            _context = context;
            _rssService = rssService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            var userDto = new UserDto
            {
                Username = user.Username,
                Email = user.Email
            };
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thông tin thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa người dùng thành công" });
        }

        //follow tag
        [HttpPost("{userId}/follow/{tagId}")]
        public async Task<IActionResult> FollowTag(int userId, int tagId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null)
            {
                return BadRequest(new { message = "Tag không hợp lệ" });
            }

            // Kiểm tra xem người dùng đã theo dõi tag này chưa
            if (_context.UserTags.Any(ut => ut.UserId == userId && ut.TagId == tagId))
            {
                return BadRequest(new { message = "Người dùng đã theo dõi tag này" });
            }

            var userTag = new User_Tags
            {
                UserId = userId,
                TagId = tagId
            };
            await _context.UserTags.AddAsync(userTag);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã theo dõi tag thành công" });
        }

        [HttpGet("{userId}/followed-tags")]
        public async Task<IActionResult> GetFollowedTags(int userId)
        {
            // Kiểm tra xem UserId có tồn tại trong cơ sở dữ liệu hay không
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            var followedTags = await _context.UserTags
                .Where(ut => ut.UserId == userId)
                .Select(ut => new TagDto
                {
                    Id = ut.Tag.Id,
                    Name = ut.Tag.Name
                }).ToListAsync();

            return Ok(followedTags);
        }

    }
}
