using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;

namespace DailyNews.Services
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ArticleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Articles>> GetArticlesAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Articles> GetArticleByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<Articles> CreateArticleAsync(ArticleDto articleDto)
        {
            // Kiểm tra RssCategory hợp lệ
            var rssCategory = await _context.RssCategories
                .Include(c => c.RssSource) // Kết nối với bảng RssSource
                .Include(c => c.Category)   // Kết nối với bảng Category
                .FirstOrDefaultAsync(c => c.Id == articleDto.RssCategoryId);

            if (rssCategory == null)
            {
                return null; // RssCategory không tồn tại
            }

            var article = _mapper.Map<Articles>(articleDto);
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            return article; // Trả về bài viết đã tạo
        }

        public async Task<bool> UpdateArticleAsync(int id, ArticleDto articleDto)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return false; // Bài viết không tồn tại
            }

            _mapper.Map(articleDto, article); // Cập nhật thông tin từ DTO vào entity

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true; // Cập nhật thành công
            }
            catch (DbUpdateConcurrencyException)
            {
                return false; // Xảy ra lỗi trong quá trình cập nhật
            }
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return false; // Bài viết không tồn tại
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true; // Xóa thành công
        }

        public async Task<bool> ArticleExistsAsync(int id)
        {
            return await _context.Articles.AnyAsync(e => e.Id == id);
        }
    }
}
