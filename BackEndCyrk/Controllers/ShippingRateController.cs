using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingRateController : Controller {
        private readonly IShippingRateRepository _shippingRateRepository;

        public ShippingRateController(IShippingRateRepository shippingRepository) {
            _shippingRateRepository = shippingRepository;
        }

        // GET: api/ShippingRate
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShippingRate>>> GetAllShippingRates() {
            var shippings = await _shippingRateRepository.GetAllAsync();
            return Ok(shippings);
        }

        // GET: api/ShippingRate/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShippingRate>> GetShippingRate(int id) {
            var shipping = await _shippingRateRepository.GetByIdAsync(id);

            if (shipping == null) {
                return NotFound();
            }

            return Ok(shipping);
        }

        // GET: api/ShippingRate/carrier/DHL
        [HttpGet("carrier/{carrier}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShippingRate>>> GetRatesByCarrier(string carrier) {
            var rates = await _shippingRateRepository.GetByCarrier(carrier);
            return Ok(rates);
        }

        // GET: api/ShippingRate/package/Standard
        [HttpGet("package/{packageType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShippingRate>>> GetRatesByPackageType(string packageType) {
            var rates = await _shippingRateRepository.GetByPackageType(packageType);
            return Ok(rates);
        }

        // GET: api/ShippingRate/carrier/DHL/package/Standard
        [HttpGet("carrier/{carrier}/package/{packageType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShippingRate>> GetRateByCarrierAndPackageType(string carrier, string packageType) {
            var rate = await _shippingRateRepository.GetByCarrierAndPackageType(carrier, packageType);

            if (rate == null) {
                return NotFound();
            }

            return Ok(rate);
        }

        // GET: api/ShippingRate/price-range/10/20
        [HttpGet("price-range/{minPrice}/{maxPrice}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShippingRate>>> GetRatesByPriceRange(decimal minPrice, decimal maxPrice) {
            var rates = await _shippingRateRepository.GetByPriceRange(minPrice, maxPrice);
            return Ok(rates);
        }

        // GET: api/ShippingRate/lowest
        [HttpGet("lowest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetLowestRate() {
            var lowestRate = await _shippingRateRepository.GetLowestRate();
            return Ok(lowestRate);
        }

        // GET: api/ShippingRate/highest
        [HttpGet("highest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetHighestRate() {
            var highestRate = await _shippingRateRepository.GetHighestRate();
            return Ok(highestRate);
        }

        // POST: api/ShippingRate
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShippingRate>> CreateShippingRate(ShippingRate shippingRate) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _shippingRateRepository.AddAsync(shippingRate);

            return CreatedAtAction(nameof(GetShippingRate), new { id = shippingRate.id }, shippingRate);
        }

        // PUT: api/ShippingRate/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateShippingRate(int id, ShippingRate shippingRate) {
            if (id != shippingRate.id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var existingRate = await _shippingRateRepository.GetByIdAsync(id);
            if (existingRate == null) {
                return NotFound();
            }

            await _shippingRateRepository.UpdateAsync(shippingRate);

            return NoContent();
        }

        // DELETE: api/ShippingRate/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteShippingRate(int id) {
            var shippingRate = await _shippingRateRepository.GetByIdAsync(id);
            if (shippingRate == null) {
                return NotFound();
            }

            await _shippingRateRepository.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}
