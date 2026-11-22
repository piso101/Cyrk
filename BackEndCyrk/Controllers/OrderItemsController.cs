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
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _itemRepository;

        public OrderItemsController(IOrderItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET: api/OrderItems
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemRepository.GetAllAsync();
            var result = items.Select(i => new OrderItemReadDto
            {
                id = i.id,
                order_id = i.order_id,
                product_id = i.product_id,
                quantity = i.quantity,
                price = i.price,
                created_at = i.created_at
            });
            return Ok(result);
        }

        // GET: api/OrderItems/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var i = await _itemRepository.GetByIdAsync(id);
            if (i == null) return NotFound();

            var dto = new OrderItemReadDto
            {
                id = i.id,
                order_id = i.order_id,
                product_id = i.product_id,
                quantity = i.quantity,
                price = i.price,
                created_at = i.created_at
            };
            return Ok(dto);
        }

        // GET: api/OrderItems/order/3
        [Authorize]
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var items = await _itemRepository.GetByOrderId(orderId);
            var result = items.Select(i => new OrderItemReadDto
            {
                id = i.id,
                order_id = i.order_id,
                product_id = i.product_id,
                quantity = i.quantity,
                price = i.price,
                created_at = i.created_at
            });
            return Ok(result);
        }

        // POST: api/OrderItems
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderItemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var item = new OrderItem
            {
                order_id = dto.order_id,
                product_id = dto.product_id,
                quantity = dto.quantity,
                price = dto.price
            };

            var created = await _itemRepository.AddAsync(item);

            var readDto = new OrderItemReadDto
            {
                id = created.id,
                order_id = created.order_id,
                product_id = created.product_id,
                quantity = created.quantity,
                price = created.price,
                created_at = created.created_at
            };

            return CreatedAtAction(nameof(GetById), new { id = readDto.id }, readDto);
        }

        // PUT: api/OrderItems/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderItemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _itemRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.quantity = dto.quantity;
            existing.price = dto.price;

            await _itemRepository.UpdateAsync(existing);
            return NoContent();
        }

        // DELETE: api/OrderItems/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _itemRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _itemRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
