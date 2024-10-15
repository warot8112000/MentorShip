using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;

namespace DailyNews.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Users> LoginAsync(LoginDto loginDto)
        {
            // Tìm kiếm người dùng dựa trên username và password
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.PasswordHash == loginDto.Password);
        }

        public async Task<Users> RegisterAsync(RegisterDto registerDto)
        {
            // Kiểm tra xem người dùng đã tồn tại hay chưa
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDto.Username);

            if (existingUser != null)
            {
                return null; // Người dùng đã tồn tại
            }

            var newUser = new Users
            {
                Username = registerDto.Username,
                PasswordHash = registerDto.Password,
                Email = registerDto.Email
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser; // Trả về người dùng mới được tạo
        }
    }
}
