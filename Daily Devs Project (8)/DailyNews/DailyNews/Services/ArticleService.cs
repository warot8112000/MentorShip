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
            var rssCategory = await _context.RssCategories
                .Include(c => c.RssSource) 
                .Include(c => c.Category)   
                .FirstOrDefaultAsync(c => c.Id == articleDto.RssCategoryId);

            if (rssCategory == null)
            {
                return null;
            }

            var article = _mapper.Map<Articles>(articleDto);
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            return article; 
        }

        public async Task<bool> UpdateArticleAsync(int id, ArticleDto articleDto)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return false; 
            }

            _mapper.Map(articleDto, article); 

            _context.Entry(article).State = EntityState.Modified;

            var affectedRows = await _context.SaveChangesAsync(); 

            if (affectedRows == 0)
            {
                throw new Exception("No records were updated."); 
            }
            return true;
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return false; 
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> ArticleExistsAsync(int id)
        {
            return await _context.Articles.AnyAsync(e => e.Id == id);
        }
    }
}
