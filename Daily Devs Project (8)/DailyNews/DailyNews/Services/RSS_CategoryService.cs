using DailyNews.DTO;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNews.Services
{
    public class RSS_CategoryService
    {
        private readonly ApplicationDbContext _context;

        public RSS_CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trả về danh sách RSS_Category dạng IQueryable cho OData
        public IQueryable<RSS_Category> GetRSS_CategoriesQueryable()
        {
            return _context.RssCategories.AsQueryable();
        }

        // Tìm một RSS_Category theo ID
        public async Task<RSS_Category> GetRSS_CategoryByIdAsync(int id)
        {
            return await _context.RssCategories.FindAsync(id);
        }

        // Thêm mới một RSS_Category
        public async Task<bool> AddRSS_CategoryAsync(RSS_Category category)
        {
            _context.RssCategories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<RSS_Category>> GetCategoriesBySourceIdAsync(int sourceId)
        {
            return await _context.RssCategories
                .Where(c => c.RssSourceId == sourceId)
                .Include(c => c.RssSource) 
                .ToListAsync();
        }

        // Cập nhật RSS_Category
        public async Task<bool> UpdateRSS_CategoryAsync(int id, RSS_Category updatedCategory)
        {
            var existingCategory = await _context.RssCategories.FindAsync(id);
            if (existingCategory == null)
            {
                return false;
            }
            existingCategory.RssSourceId = updatedCategory.RssSourceId;       
            existingCategory.CategoryId = updatedCategory.CategoryId;    
            existingCategory.Url = updatedCategory.Url;   
            existingCategory.Description = updatedCategory.Description;

            _context.RssCategories.Update(existingCategory);
            return await _context.SaveChangesAsync() > 0;
        }

        // Xóa RSS_Category
        public async Task<bool> DeleteRSS_CategoryByIdAsync(int id)
        {
            var category = await _context.RssCategories.FindAsync(id);
            if (category == null)
            {
                return false;
            }

            _context.RssCategories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
