using AutoMapper;
using DailyNews.DTO;
using DailyNews.Model;
using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Articles>>> GetArticles()
        {
            var articles = await _articleService.GetArticlesAsync();
            return Ok(articles);
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
