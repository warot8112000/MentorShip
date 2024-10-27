using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using DailyNews.Response;
using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailyNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _articleService;

        public ArticlesController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("odata")]
        [EnableQuery] 
        public ActionResult<IQueryable<ArticleDto>> GetArticlesOData()
        {
            var articlesQueryable = _articleService.GetArticlesQueryable();

            return Ok(articlesQueryable);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResponse<ArticleDto>>> GetPaginatedArticles(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (pageSize < 10 || pageSize > 20) 
            {
                pageSize = 10;
            }
            var totalArticles = await _articleService.GetTotalArticle();
            var articles = await _articleService.GetArticlesAsync(pageNumber, pageSize);

            var response = new PagedResponse<ArticleDto>(totalArticles, pageNumber, pageSize, articles);

            return Ok(response);
        }

        // GET: api/article/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Articles>> GetArticle(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        // POST: api/article
        [HttpPost]
        public async Task<ActionResult<Articles>> CreateArticle([FromBody] ArticleDto articleDto)
        {
            var article = await _articleService.CreateArticleAsync(articleDto);
            if (article == null)
            {
                return NotFound("RssCategory not found.");
            }

            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
        }

        // PUT: api/article/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, ArticleDto articleDto)
        {
            var result = await _articleService.UpdateArticleAsync(id, articleDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var result = await _articleService.DeleteArticleAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("old-articles/{date}")]
        public async Task<IActionResult> DeleteOldArticles(DateTime date)
        {
            var count = _articleService.DeleteArticlesOlderThan(date);

            return Ok(new { message = $"{count} bài báo cũ hơn {date} đã được xóa." });
        }

        [HttpDelete("by-category/{categoryId}")]
        public async Task<IActionResult> DeleteArticlesByCategory(int categoryId)
        {
            var count = _articleService.DeleteArticlesByCategory(categoryId);

            return Ok(new { message = $"{count} bài báo thuộc danh mục {categoryId} đã được xóa." });
        }
    }
}
