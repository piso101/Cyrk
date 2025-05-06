using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/Category
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/Category/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET: api/Category/1/products
        [HttpGet("{id}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategoryWithProducts(int id)
        {
            var category = await _categoryRepository.GetWithProductsAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET: api/Category/search/name
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> SearchCategoriesByName(string searchTerm)
        {
            var categories = await _categoryRepository.SearchByNameAsync(searchTerm);
            return Ok(categories);
        }

        // POST: api/Category
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCategory = await _categoryRepository.AddAsync(category);

            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.id }, createdCategory);
        }

        // PUT: api/Category/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.id)
            {
                return BadRequest();
            }

            var exists = await _categoryRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var success = await _categoryRepository.UpdateAsync(category);
            if (!success)
            {
                return BadRequest("Failed to update category");
            }

            return NoContent();
        }

        // DELETE: api/Category/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var exists = await _categoryRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var success = await _categoryRepository.DeleteAsync(id);
            if (!success)
            {
                return BadRequest("Failed to delete category");
            }

            return NoContent();
        }
    }
}