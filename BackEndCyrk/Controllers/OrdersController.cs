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
    /// <summary>
    /// Kontroler obs³uguj¹cy operacje na zamówieniach
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        /// <summary>
        /// Pobiera wszystkie zamówienia
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all orders");

                var orders = await _orderRepository.GetAllAsync();
                var result = orders.Select(o => new OrderReadDto
                {
                    id = o.id,
                    user_id = o.user_id,
                    total_price = o.total_price,
                    status = o.status,
                    created_at = o.created_at
                });

                _logger.LogInformation($"Found {orders.Count()} orders");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all orders");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Pobiera zamówienie po ID
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting order with ID: {id}");

                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found");
                    return NotFound($"Order with ID {id} not found");
                }

                var dto = new OrderReadDto
                {
                    id = order.id,
                    user_id = order.user_id,
                    total_price = order.total_price,
                    status = order.status,
                    created_at = order.created_at
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting order with ID: {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Pobiera zamówienie z pe³nymi szczegó³ami (user, items, payment, shipping)
        /// </summary>
        [Authorize]
        [HttpGet("{id}/details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWithDetails(int id)
        {
            try
            {
                _logger.LogInformation($"Getting order details for ID: {id}");

                var order = await _orderRepository.GetWithDetailsAsync(id);
                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting order details for ID: {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Pobiera zamówienia po ID u¿ytkownika
        /// </summary>
        [Authorize]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                _logger.LogInformation($"Getting orders for user ID: {userId}");

                var orders = await _orderRepository.GetByUserIdAsync(userId);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting orders for user ID: {userId}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Pobiera zamówienia po statusie
        /// </summary>
        [Authorize]
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStatus(string status)
        {
            try
            {
                _logger.LogInformation($"Getting orders with status: {status}");

                var orders = await _orderRepository.GetByStatusAsync(status);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting orders with status: {status}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Tworzy nowe zamówienie
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation($"Creating new order for user ID: {dto.user_id}");

                var order = new Order
                {
                    user_id = dto.user_id,
                    total_price = dto.total_price,
                    status = dto.status
                };

                var createdOrder = await _orderRepository.AddAsync(order);

                var readDto = new OrderReadDto
                {
                    id = createdOrder.id,
                    user_id = createdOrder.user_id,
                    total_price = createdOrder.total_price,
                    status = createdOrder.status,
                    created_at = createdOrder.created_at
                };

                return CreatedAtAction(nameof(GetById), new { id = readDto.id }, readDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizuje zamówienie
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation($"Updating order with ID: {id}");

                var existing = await _orderRepository.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound($"Order with ID {id} not found");
                }

                existing.total_price = dto.total_price;
                existing.status = dto.status;

                await _orderRepository.UpdateAsync(existing);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating order with ID: {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Usuwa zamówienie
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting order with ID: {id}");

                var exists = await _orderRepository.ExistsAsync(o => o.id == id);
                if (!exists)
                {
                    return NotFound($"Order with ID {id} not found");
                }

                await _orderRepository.DeleteByIdAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting order with ID: {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Aktualizuje status zamówienia
        /// </summary>
        [Authorize]
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            try
            {
                _logger.LogInformation($"Updating status for order ID: {id} to: {status}");

                var exists = await _orderRepository.ExistsAsync(o => o.id == id);
                if (!exists)
                {
                    return NotFound($"Order with ID {id} not found");
                }

                await _orderRepository.UpdateStatus(id, status);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating status for order ID: {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Pobiera ³¹czny przychód z zamówieñ
        /// </summary>
        [Authorize]
        [HttpGet("revenue/total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                _logger.LogInformation("Getting total revenue");

                var totalRevenue = await _orderRepository.GetTotalRevenueAsync();
                return Ok(new { totalRevenue = totalRevenue });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total revenue");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Pobiera zamówienia z okreœlonego zakresu dat
        /// </summary>
        [Authorize]
        [HttpGet("daterange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                _logger.LogInformation($"Getting orders from {startDate} to {endDate}");

                var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting orders by date range");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}