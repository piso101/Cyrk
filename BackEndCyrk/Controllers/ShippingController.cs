using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : Controller
    {
        private readonly IShippingRepository _shippingRepository;

        public ShippingController(IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        // GET: api/Shipping
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetAllShippings()
        {
            var shippings = await _shippingRepository.GetAllAsync();
            return Ok(shippings);
        }

        // GET: api/Shipping/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shipping>> GetShipping(int id)
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
