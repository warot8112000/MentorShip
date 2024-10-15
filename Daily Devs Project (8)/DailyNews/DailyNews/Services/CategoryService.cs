using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;

namespace DailyNews.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return null; // Không tìm thấy category
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false; // Không tìm thấy category
            }

            _mapper.Map(categoryDto, category);
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) // xung đột đồng thời
            {
                if (!CategoryExists(id))
                {
                    return false; 
                }
                throw;
            }

            return true; // Cập nhật thành công
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false; // Không tìm thấy category
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true; // Xóa thành công
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
