using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEndCyrk.Models;
using BackEndCyrk.Repositories;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _itemRepository;

        public OrderItemsController(IOrderItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemRepository.GetAllAsync();
            return Ok(items);
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // GET: api/OrderItems/order/3
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var items = await _itemRepository.GetByOrderId(orderId);
            return Ok(items);
        }

        // POST: api/OrderItems
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderItem item)
        {
            var created = await _itemRepository.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }

        // PUT: api/OrderItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderItem item)
        {
            if (id != item.id) return BadRequest();
            await _itemRepository.UpdateAsync(item);
            return NoContent();
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
