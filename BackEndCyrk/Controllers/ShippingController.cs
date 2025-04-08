using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : Controller {
        private readonly IShippingRepository _shippingRepository;

        public ShippingController(IShippingRepository shippingRepository) {
            _shippingRepository = shippingRepository;
        }

        // GET: api/Shipping
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetAllShippings() {
            var shippings = await _shippingRepository.GetAllAsync();
            return Ok(shippings);
        }

        // GET: api/Shipping/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> GetShipping(int id) {
            var shipping = await _shippingRepository.GetByIdAsync(id);

            if (shipping == null) {
                return NotFound();
            }

            return Ok(shipping);
        }

        // GET api/Shipping/order/1
        [HttpGet("order/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> GetShippingByOrder(int orderId) {
            var shipping = await _shippingRepository.GetByOrderId(orderId);

            if (shipping == null) {
                return NotFound();
            }

            return Ok(shipping);
        }

        // GET api/Shipping/status/Pending
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> GetShippingByStatus(int id) {
            var shipping = await _shippingRepository.GetByIdAsync(id);

            if (shipping == null) {
                return NotFound();
            }

            return Ok(shipping);
        }

        // GET api/Shipping/carrier/DHL
        [HttpGet("carrier/{carrier}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> GetShippingByCarrier(string carrier) {
            var shipping = await _shippingRepository.GetByCarrier(carrier);

            if (shipping == null) {
                return NotFound();
            }

            return Ok(shipping);
        }

        // POST /api/Shipping
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Shipping>> CreateShipping(Shipping shipping) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var createdShipping = await _shippingRepository.AddAsync(shipping);
            return CreatedAtAction(nameof(GetShipping), new { id = createdShipping.id }, createdShipping);
        }

        // PUT api/Shipping/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> UpdateShipping(int id, Shipping updatedShipping) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var shipping = await _shippingRepository.GetByIdAsync(id);
            if (shipping == null) {
                return NotFound();
            }

            updatedShipping.id = id; // Nie jestem pewien czy to jest potrzebne

            await _shippingRepository.UpdateAsync(updatedShipping);

            return NoContent();
        }

        // PATCH api/Shipping/1/status/Shipped
        [HttpPatch("{id}/status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> UpdateShippingStatus(int id, string status) {
            var shipping = await _shippingRepository.GetByIdAsync(id);

            if (shipping == null) {
                return NotFound();
            }

            shipping.status = status;
            await _shippingRepository.UpdateStatus(id, status);

            return NoContent();
        }

        // PATCH api/Shipping/1/tracking/AB123456789
        [HttpPatch("{id}/tracking/{trackNum}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> UpdateShippingTrackNumber(int id, string trackNum) {
            var shipping = await _shippingRepository.GetByIdAsync(id);

            if (shipping == null) {
                return NotFound();
            }

            await _shippingRepository.UpdateTrackNumber(id, trackNum);

            return NoContent();
        }

        // PATCH api/Shipping/1/delivery
        [HttpGet("{id}/delivery")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> UpdateShippingDelivery(int id, [FromBody] DateTime deliveryTime) {
            var shipping = await _shippingRepository.GetByIdAsync(id);

            if (shipping == null) {
                return NotFound();
            }

            await _shippingRepository.UpdateDelivery(id, deliveryTime);

            return NoContent();
        }

        // DELETE /api/Shipping/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> DeleteShipping(int id) {
            var shipping = await _shippingRepository.GetByIdAsync(id);

            if (shipping == null) {
                return NotFound();
            }
            await _shippingRepository.DeleteByIdAsync(id);

            return NoContent();
        }


    }
}
