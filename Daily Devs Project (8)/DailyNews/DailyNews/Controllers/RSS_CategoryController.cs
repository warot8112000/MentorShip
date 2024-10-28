using DailyNews.Model;
using DailyNews.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RSS_CategoryController : ControllerBase
    {
        private readonly RSS_CategoryService _categoryService;

        public RSS_CategoryController(RSS_CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Lấy danh sách RSS_Category, hỗ trợ OData
        [HttpGet("odata")]
        [EnableQuery] 
        public IActionResult GetRSS_Categories()
        {
            var categories = _categoryService.GetRSS_CategoriesQueryable();
            return Ok(categories);
        }

        // Lấy một RSS_Category theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRSS_CategoryById(int id)
        {
            var category = await _categoryService.GetRSS_CategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Danh mục không tồn tại" });
            }

            return Ok(category);
        }

        [HttpGet("{sourceId}/categories")]
        public async Task<IActionResult> GetCategoriesBySourceId(int sourceId)
        {
            var categories = await _categoryService.GetCategoriesBySourceIdAsync(sourceId);

            if (categories == null || !categories.Any())
            {
                return NotFound(new { message = "No categories found for the specified RSS source." });
            }

            return Ok(categories);
        }

        // Thêm mới một RSS_Category
        [HttpPost]
        public async Task<IActionResult> AddRSS_Category([FromBody] RSS_Category newCategory)
        {
            var result = await _categoryService.AddRSS_CategoryAsync(newCategory);
            if (!result)
            {
                return BadRequest(new { message = "Thêm danh mục thất bại" });
            }

            return CreatedAtAction(nameof(GetRSS_CategoryById), new { id = newCategory.Id }, newCategory);
        }

        // Cập nhật RSS_Category theo ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRSS_Category(int id, [FromBody] RSS_Category updatedCategory)
        {
            var result = await _categoryService.UpdateRSS_CategoryAsync(id, updatedCategory);
            if (!result)
            {
                return NotFound(new { message = "Danh mục không tồn tại" });
            }

            return Ok(new { message = "Cập nhật danh mục thành công" });
        }

        // Xóa RSS_Category theo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRSS_CategoryById(int id)
        {
            var result = await _categoryService.DeleteRSS_CategoryByIdAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Danh mục không tồn tại" });
            }

            return Ok(new { message = "Xóa danh mục thành công" });
        }
    }
}
