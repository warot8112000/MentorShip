using DailyNews.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNews.Services
{
    public class RSS_SourcesService
    {
        private readonly ApplicationDbContext _context;

        public RSS_SourcesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách RSS_Sources dưới dạng IQueryable để hỗ trợ OData
        public IQueryable<RSS_Sources> GetRSS_SourcesQueryable()
        {
            return _context.RssSources.AsQueryable();
        }

        // Lấy một nguồn RSS theo ID
        public async Task<RSS_Sources> GetRSS_SourceByIdAsync(int id)
        {
            return await _context.RssSources.FindAsync(id);
        }

        // Thêm một nguồn RSS mới
        public async Task<bool> AddRSS_SourceAsync(RSS_Sources rssSource)
        {
            _context.RssSources.Add(rssSource);
            return await _context.SaveChangesAsync() > 0;
        }

        // Cập nhật thông tin nguồn RSS
        public async Task<bool> UpdateRSS_SourceAsync(int id, RSS_Sources updatedRSSSource)
        {
            var existingRSSSource = await _context.RssSources.FindAsync(id);
            if (existingRSSSource == null)
            {
                return false;
            }

            existingRSSSource.Name = updatedRSSSource.Name;
            existingRSSSource.Url = updatedRSSSource.Url;
            existingRSSSource.Description = updatedRSSSource.Description;

            _context.RssSources.Update(existingRSSSource);
            return await _context.SaveChangesAsync() > 0;
        }

        // Xóa nguồn RSS theo ID
        public async Task<bool> DeleteRSS_SourceByIdAsync(int id)
        {
            var rssSource = await _context.RssSources.FindAsync(id);
            if (rssSource == null)
            {
                return false;
            }

            _context.RssSources.Remove(rssSource);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
