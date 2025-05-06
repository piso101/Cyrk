using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/Product
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        // GET: api/Product/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Product/category/1
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);
            return Ok(products);
        }

        // GET: api/Product/search/name
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsByName(string searchTerm)
        {
            var products = await _productRepository.SearchByNameAsync(searchTerm);
            return Ok(products);
        }

        // POST: api/Product
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productRepository.AddAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.id }, createdProduct);
        }

        // PUT: api/Product/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.id)
            {
                return BadRequest();
            }

            var exists = await _productRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var success = await _productRepository.UpdateAsync(product);
            if (!success)
            {
                return BadRequest("Failed to update product");
            }

            return NoContent();
        }

        // PATCH: api/Product/1/stock/10
        [HttpPatch("{id}/stock/{quantity}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductStock(int id, int quantity)
        {
            if (quantity < 0)
            {
                return BadRequest("Stock quantity cannot be negative");
            }

            var exists = await _productRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var success = await _productRepository.UpdateStockAsync(id, quantity);
            if (!success)
            {
                return BadRequest("Failed to update product stock");
            }

            return NoContent();
        }

        // DELETE: api/Product/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var exists = await _productRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var success = await _productRepository.DeleteAsync(id);
            if (!success)
            {
                return BadRequest("Failed to delete product");
            }

            return NoContent();
        }
    }
}