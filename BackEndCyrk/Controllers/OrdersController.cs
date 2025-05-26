using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/Orders
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/Orders
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            var created = await _orderRepository.AddAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }

        // PUT: api/Orders/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order)
        {
            if (id != order.id) return BadRequest();
            await _orderRepository.UpdateAsync(order);
            return NoContent();
        }

        // DELETE: api/Orders/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderRepository.DeleteByIdAsync(id);
            return NoContent();
        }

        // PATCH: api/Orders/5/status
        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            await _orderRepository.UpdateStatus(id, status);
            return NoContent();
        }
    }
}
