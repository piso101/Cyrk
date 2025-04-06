using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingRateController : Controller
    {
        private readonly IShippingRateRepository _shippingRepository;

        public ShippingRateController(IShippingRateRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        // GET: api/ShippingRate
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShippingRate>>> GetAllShippingRates()
        {
            var shippings = await _shippingRepository.GetAllAsync();
            return Ok(shippings);
        }

        // GET: api/ShippingRate/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShippingRate>> GetShippingRate(int id)
        {
            var shipping = await _shippingRepository.GetByIdAsync(id);

            if (shipping == null)
            {
                return NotFound();
            }

            return Ok(shipping);
        }


    }
}
