using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }
        [Authorize]
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            return Ok(products);
        }
        [Authorize]
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsByName(string searchTerm)
        {
            var products = await _productRepository.SearchByNameAsync(searchTerm);
            return Ok(products);
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var product = new Product
                {
                    name = productDto.Name,
                    description = productDto.Description,
                    price = productDto.Price,
                    category_id = productDto.CategoryId,
                    stock = productDto.Stock
                };

                var createdProduct = await _productRepository.AddAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.id }, createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating product: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != productDto.Id) return BadRequest("ID mismatch");

            var exists = await _productRepository.ExistsAsync(id);
            if (!exists) return NotFound();

            try
            {
                var product = new Product
                {
                    id = productDto.Id,
                    name = productDto.Name,
                    description = productDto.Description,
                    price = productDto.Price,
                    category_id = productDto.CategoryId,
                    stock = productDto.Stock
                };

                var success = await _productRepository.UpdateAsync(product);
                return success ? NoContent() : BadRequest("Update failed");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating product: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPatch("{id}/stock/{quantity}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductStock(int id, int quantity)
        {
            if (quantity < 0) return BadRequest("Stock cannot be negative");

            var exists = await _productRepository.ExistsAsync(id);
            if (!exists) return NotFound();

            var success = await _productRepository.UpdateStockAsync(id, quantity);
            return success ? NoContent() : BadRequest("Stock update failed");
        }
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var exists = await _productRepository.ExistsAsync(id);
            if (!exists) return NotFound();

            var success = await _productRepository.DeleteAsync(id);
            return success ? NoContent() : BadRequest("Deletion failed");
        }
    }

    public class ProductCreateDto
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; } = 0;
    }

    public class ProductUpdateDto : ProductCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}