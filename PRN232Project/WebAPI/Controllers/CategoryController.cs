using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace PRN232Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(int id)
        {
            var category = _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}
