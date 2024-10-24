using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

        //GET Method
        public async Task<IEnumerable<ArticleDto>> GetArticlesAsync(int pageNumber, int pageSize)
        {
            var articles = await _context.Articles
                                 .OrderByDescending(a => a.CreatedAt)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
            var articlesDto = _mapper.Map<IEnumerable<ArticleDto>>(articles);
            return articlesDto;
        }

        public async Task<List<Articles>> GetArticlesAsync()
        {
            var articlesDto = await _context.Articles.ToListAsync();
            return articlesDto;
        }
        
        public async Task<ArticleDto> GetArticleByIdAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return null; 
            }
            var articleDto = _mapper.Map<ArticleDto>(article);

            return articleDto;
        }

        public async Task<List<Articles>> GetArticlesByCategory(int categoryId)
        {
            return await _context.Articles
                .Where(a => a.RssCategoryId == categoryId) // Điều kiện theo danh mục
                .OrderByDescending(a => a.PublishedAt)
                .ToListAsync();
        }

        //POST method

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

        //PUT Method
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

        //DELETE Method
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


        public async Task<int> DeleteArticlesOlderThan(DateTime date)
        {
            var oldArticles = await _context.Articles
                .Where(a => a.PublishedAt < date)
                .ToListAsync();

            if (oldArticles.Any())
            {
                _context.Articles.RemoveRange(oldArticles);
                return await _context.SaveChangesAsync();  
            }

            return 0;  
        }

        public async Task<int> DeleteArticlesByCategory(int categoryId)
        {
            var articles = await _context.Articles
                                         .Where(a => a.RssCategoryId == categoryId)
                                         .ToListAsync();

            if (articles.Any())
            {
                _context.Articles.RemoveRange(articles);
                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        //
        public async Task<int> GetTotalArticle()
        {
            return await _context.Articles.CountAsync();
        }

        public async Task<bool> ArticleExistsAsync(int id)
        {
            return await _context.Articles.AnyAsync(e => e.Id == id);
        }
    }
}
