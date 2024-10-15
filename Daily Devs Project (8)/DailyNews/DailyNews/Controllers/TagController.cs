using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly TagService _tagService;

        public TagController(ApplicationDbContext context, TagService tagService)
        {
            _context = context;
            _tagService = tagService;
        }

        
        /*
        [HttpGet("check-tags/{articleId}")]
        public async Task<IActionResult> GetTagsFromArticle(int articleId)
        {
            // Tìm bài viết dựa trên articleId
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == articleId);
            if (article == null)
            {
                return NotFound(new { message = "Không tìm thấy bài viết" });
            }

            // Gọi service để lấy tags từ URL bài viết
            var tagsFromArticle = await _tagService.GetArticleTags(article.Url);

            if (tagsFromArticle == null || !tagsFromArticle.Any())
            {
                return NotFound(new { message = "Không tìm thấy tag nào từ bài viết" });
            }

            // Trả về danh sách tags
            return Ok(new { articleTitle = article.Title, tags = tagsFromArticle });
        }*/
    }
}

