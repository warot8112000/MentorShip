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
        private readonly UserService _userService;

        public UserController( UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userDtos = await _userService.GetUsersAsync();
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var result = await _userService.UpdateUserAsync(id, updateUserDto);
            if (!result)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            return Ok(new { message = "Cập nhật thông tin thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var result = await _userService.DeleteUserByIdAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }
            return Ok(new { message = "Xóa người dùng thành công" });
        }

        //follow tag
        [HttpPost("{userId}/follow/{tagId}")]
        public async Task<IActionResult> FollowTag(int userId, int tagId)
        {
            var result = await _userService.FollowTagAsync(userId, tagId);
            if (!result)
            {
                return BadRequest(new { message = "Người dùng không tồn tại hoặc tag không hợp lệ hoặc đã theo dõi tag này" });
            }

            return Ok(new { message = "Đã theo dõi tag thành công" });
        }

        [HttpGet("{userId}/followed-tags")]
        public async Task<IActionResult> GetFollowedTags(int userId)
        {
            // Kiểm tra xem UserId có tồn tại trong cơ sở dữ liệu hay không
            var followedTags = await _userService.GetFollowedTagsAsync(userId);
            if (followedTags == null)
            {
                return NotFound(new { message = "Người dùng không tồn tại" });
            }

            return Ok(followedTags);
        }

    }
}
