using DailyNews.Model;
using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RSS_SourcesController : ODataController
    {
        private readonly RSS_SourcesService _rssSourcesService;

        public RSS_SourcesController(RSS_SourcesService rssSourcesService)
        {
            _rssSourcesService = rssSourcesService;
        }

        // OData-enabled Get RSS_Sources
        [HttpGet("odata")]
        [EnableQuery] 
        public ActionResult<IQueryable<RSS_Sources>> GetRSS_Sources()
        {
            // Trả về dữ liệu dạng IQueryable cho phép OData áp dụng các query options
            var rssSourcesQueryable = _rssSourcesService.GetRSS_SourcesQueryable();
            return Ok(rssSourcesQueryable);
        }

        // GET api/RSS_Sources/{id} - lấy một nguồn RSS theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRSS_SourceById(int id)
        {
            var rssSource = await _rssSourcesService.GetRSS_SourceByIdAsync(id);
            if (rssSource == null)
            {
                return NotFound(new { message = "Nguồn RSS không tồn tại" });
            }

            return Ok(rssSource);
        }

        // POST api/RSS_Sources - thêm một nguồn RSS mới
        [HttpPost]
        public async Task<IActionResult> CreateRSS_Source([FromBody] RSS_Sources rssSource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _rssSourcesService.AddRSS_SourceAsync(rssSource);
            return CreatedAtAction(nameof(GetRSS_SourceById), new { id = rssSource.Id }, rssSource);
        }

        // PUT api/RSS_Sources/{id} - cập nhật thông tin một nguồn RSS
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRSS_Source(int id, [FromBody] RSS_Sources rssSource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _rssSourcesService.UpdateRSS_SourceAsync(id, rssSource);
            if (!result)
            {
                return NotFound(new { message = "Nguồn RSS không tồn tại" });
            }

            return Ok(new { message = "Cập nhật thông tin thành công" });
        }

        // DELETE api/RSS_Sources/{id} - xóa một nguồn RSS
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRSS_SourceById(int id)
        {
            var result = await _rssSourcesService.DeleteRSS_SourceByIdAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Nguồn RSS không tồn tại" });
            }

            return Ok(new { message = "Xóa nguồn RSS thành công" });
        }
    }
}
