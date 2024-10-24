using AutoMapper;
using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FetchDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly FetchDataService _rssService;
        private readonly IMapper _mapper;
        public FetchDataController(ApplicationDbContext context, FetchDataService rssService, IMapper mapper)
        {
            _context = context;
            _rssService = rssService;
            _mapper = mapper;
        }

        //get RSSCategories -> Articles -> Save in db
        [HttpPost("Articles")]
        public async Task<IActionResult> FetchArticles()
        {
            await _rssService.FetchAndSaveArticlesFromRssCategories();
            return Ok("Articles fetched and stored successfully."); 
        }

        [HttpPost("RssCategories")]
        public async Task<IActionResult> FetchRssCategories()
        {
            await _rssService.GetRssCategoriesFromRssSources();
            return Ok("RssCategories fetched and stored successfully.");
        }

        /* [HttpPost("tags")]
         public async Task<IActionResult> AddTagsToArticles()
         {
             try
             {
                 await _rssService.AddTagsToArticles();
                 return Ok(new { message = "Đã thêm tag cho tất cả các bài viết." });
             }
             catch (Exception ex)
             {
                 return BadRequest(new { message = ex.Message });
             }
         }*/
    }
}
