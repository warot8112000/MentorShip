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
    public class ArticlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RssFeedService _rssService;
        private readonly IMapper _mapper;

        public ArticlesController(ApplicationDbContext context, RssFeedService rssService, IMapper mapper)
        {
            _context = context;
            _rssService = rssService;
            _mapper = mapper;
        }

        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Articles>>> GetArticles()
        {
            return await _context.Articles.ToListAsync();
        }

        // GET: api/article/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Articles>> GetArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return article;
        }
        //Lấy dữ liệu từ RSSCategories -> Articles -> Save in db
        [HttpPost("fetchArticles")]
        public async Task<IActionResult> FetchArticles()
        {
            await _rssService.FetchAndSaveArticlesFromRssCategories(); // Gọi dịch vụ để lấy bài viết
            return Ok("Articles fetched and stored successfully."); // Trả về thông báo thành công
        }

        [HttpPost("fetchRssCategories")]
        public async Task<IActionResult> FetchRssCategories()
        {
            await _rssService.GetRssCategoriesFromRssSources();
            return Ok("RssCategories fetched and stored successfully.");
        }

        // POST: api/article
        [HttpPost]
        public async Task<ActionResult<Articles>> CreateArticle([FromBody] ArticleDto articleDto)
        {
            //check RSS category hop le
            var rssCategory = await _context.RssCategories
             .Include(c => c.RssSource)  // Kết nối với bảng RssSource
             .Include(c => c.Category)     // Kết nối với bảng Category
             .FirstOrDefaultAsync(c => c.Id == articleDto.RssCategoryId);

            if (rssCategory == null)
            {
                return NotFound("RssCategory not found.");
            }

            var article = _mapper.Map<Articles>(articleDto);


            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);

        }

        // PUT: api/article/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, ArticleDto articleDto)
        {            

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _mapper.Map(articleDto, article); // Cập nhật thông tin từ DTO vào entity

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }

   
}
