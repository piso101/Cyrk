using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using BackEndCyrk.DTOs;
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
            var result = orders.Select(o => new OrderReadDto
            {
                id = o.id,
                user_id = o.user_id,
                total_price = o.total_price,
                status = o.status,
                created_at = o.created_at
            });
            return Ok(result);
        }

        // GET: api/Orders/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var o = await _orderRepository.GetByIdAsync(id);
            if (o == null) return NotFound();

            var dto = new OrderReadDto
            {
                id = o.id,
                user_id = o.user_id,
                total_price = o.total_price,
                status = o.status,
                created_at = o.created_at
            };
            return Ok(dto);
        }

        // POST: api/Orders
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = new Order
            {
                user_id = dto.user_id,
                total_price = dto.total_price,
                status = dto.status
            };

            var created = await _orderRepository.AddAsync(order);

            var readDto = new OrderReadDto
            {
                id = created.id,
                user_id = created.user_id,
                total_price = created.total_price,
                status = created.status,
                created_at = created.created_at
            };

            return CreatedAtAction(nameof(GetById), new { id = readDto.id }, readDto);
        }

        // PUT: api/Orders/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _orderRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.total_price = dto.total_price;
            existing.status = dto.status;

            await _orderRepository.UpdateAsync(existing);
            return NoContent();
        }

        // DELETE: api/Orders/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _orderRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _orderRepository.DeleteByIdAsync(id);
            return NoContent();
        }

        // PATCH: api/Orders/5/status
        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var existing = await _orderRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _orderRepository.UpdateStatus(id, status);
            return NoContent();
        }
    }
}
