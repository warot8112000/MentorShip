using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;

namespace DailyNews.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false; // Người dùng không tồn tại
            }

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true; // Cập nhật thành công
        }

        public async Task<bool> DeleteUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false; // Người dùng không tồn tại
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true; // Xóa thành công
        }

        public async Task<bool> FollowTagAsync(int userId, int tagId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false; // Người dùng không tồn tại
            }

            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null)
            {
                return false; // Tag không hợp lệ
            }

            // Kiểm tra xem người dùng đã theo dõi tag này chưa
            if (await _context.UserTags.AnyAsync(ut => ut.UserId == userId && ut.TagId == tagId))
            {
                return false; // Người dùng đã theo dõi tag này
            }

            var userTag = new User_Tags
            {
                UserId = userId,
                TagId = tagId
            };
            await _context.UserTags.AddAsync(userTag);
            await _context.SaveChangesAsync();
            return true; // Theo dõi tag thành công
        }

        public async Task<List<TagDto>> GetFollowedTagsAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return null; // Người dùng không tồn tại
            }

            var followedTags = await _context.UserTags
                .Where(ut => ut.UserId == userId)
                .Select(ut => new TagDto
                {
                    Id = ut.Tag.Id,
                    Name = ut.Tag.Name
                }).ToListAsync();

            return followedTags;
        }
    }
}
