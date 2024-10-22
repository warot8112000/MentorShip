using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using DailyNews.Response;
using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<ActionResult<PagedResponse<ArticleDto>>> GetArticles(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                return BadRequest("Page number must be greater than 0.");
            }
            if (pageSize < 1 || pageSize > 20) 
            {
                return BadRequest("Page size must be between 1 and 20.");
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
    }
}
