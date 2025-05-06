using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // GET: api/Inventory
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetAllInventories()
        {
            var items = await _inventoryRepository.GetAllAsync();
            return Ok(items);
        }

        // GET: api/Inventory/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inv = await _inventoryRepository.GetByIdAsync(id);
            if (inv == null)
                return NotFound();
            return Ok(inv);
        }

        // GET: api/Inventory/product/3
        [HttpGet("product/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Inventory>> GetByProduct(int productId)
        {
            var inv = await _inventoryRepository.GetByProductIdAsync(productId);
            if (inv == null)
                return NotFound();
            return Ok(inv);
        }

        // GET: api/Inventory/lowstock/10
        [HttpGet("lowstock/{threshold}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetLowStock(int threshold)
        {
            var list = await _inventoryRepository.GetLowStockAsync(threshold);
            return Ok(list);
        }

        // GET: api/Inventory/updatedafter/2025-05-01T00:00:00Z
        [HttpGet("updatedafter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetUpdatedAfter([FromQuery] DateTime since)
        {
            var list = await _inventoryRepository.GetUpdatedAfterAsync(since);
            return Ok(list);
        }

        // POST: api/Inventory
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Inventory>> CreateInventory(Inventory inventory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _inventoryRepository.AddAsync(inventory);
            return CreatedAtAction(nameof(GetInventory), new { id = created.id }, created);
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateInventory(int id, Inventory inventory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _inventoryRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            inventory.id = id;
            await _inventoryRepository.UpdateAsync(inventory);
            return NoContent();
        }

        // PATCH: api/Inventory/5/quantity/20
        [HttpPatch("{id}/quantity/{quantity}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var existing = await _inventoryRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _inventoryRepository.UpdateQuantityAsync(id, quantity);
            return NoContent();
        }

        // PATCH: api/Inventory/5/lastupdated
        [HttpPatch("{id}/lastupdated")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLastUpdated(int id, [FromBody] DateTime lastUpdated)
        {
            var existing = await _inventoryRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _inventoryRepository.UpdateLastUpdatedAsync(id, lastUpdated);
            return NoContent();
        }

        // DELETE: api/Inventory/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var existing = await _inventoryRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _inventoryRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
