using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;
namespace DailyNews.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly RssFeedService _rssService;

        public ArticlesController(RssFeedService rssService)
        {
            _rssService = rssService;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> FetchArticles()
        {
            await _rssService.FetchAndSaveArticlesFromRssCategories(); // Gọi dịch vụ để lấy bài viết
            return Ok("Articles fetched and stored successfully."); // Trả về thông báo thành công
        }
    }
}
