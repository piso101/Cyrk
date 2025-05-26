using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Test works");
        }
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
        [Authorize]
        [HttpGet("{id}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategoryWithProducts(int id)
        {
            var category = await _categoryRepository.GetWithProductsAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
        [Authorize]
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> SearchCategoriesByName(string searchTerm)
        {
            var categories = await _categoryRepository.SearchByNameAsync(searchTerm);
            return Ok(categories);
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _categoryRepository.GetByNameAsync(categoryDto.Name);
            if (existingCategory != null)
            {
                return Conflict("Category with this name already exists");
            }

            var category = new Category
            {
                name = categoryDto.Name,
                description = categoryDto.Description
            };

            try
            {
                var createdCategory = await _categoryRepository.AddAsync(category);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.id }, createdCategory);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create category: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.id) return BadRequest();
            if (!await _categoryRepository.ExistsAsync(id)) return NotFound();
            if (!await _categoryRepository.UpdateAsync(category)) return BadRequest("Failed to update category");
            return NoContent();
        }
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!await _categoryRepository.ExistsAsync(id)) return NotFound();
            if (!await _categoryRepository.DeleteAsync(id)) return BadRequest("Failed to delete category");
            return NoContent();
        }
    }

    public class CategoryCreateDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}