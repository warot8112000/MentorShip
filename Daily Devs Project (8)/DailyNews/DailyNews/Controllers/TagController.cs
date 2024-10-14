using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly TagService _tagService;

        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("{url}/tags")]
        public async Task<IActionResult> GetTags(string url)
        {
            var tags = await _tagService.GetArticleTags(url);
            return Ok(tags);
        }
    }
}

