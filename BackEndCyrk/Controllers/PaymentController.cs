// PaymentController.cs
using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // GET: api/Payment
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return Ok(payments);
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        // GET: api/Payment/order/5
        [HttpGet("order/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Payment>> GetByOrder(int orderId)
        {
            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        // GET: api/Payment/status/Pending
        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Payment>>> GetByStatus(string status)
        {
            var payments = await _paymentRepository.GetByStatusAsync(status);
            return Ok(payments);
        }

        // GET: api/Payment/method/CreditCard
        [HttpGet("method/{method}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Payment>>> GetByMethod(string method)
        {
            var payments = await _paymentRepository.GetByPaymentMethodAsync(method);
            return Ok(payments);
        }

        // POST: api/Payment
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Payment>> CreatePayment(Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _paymentRepository.AddAsync(payment);
            return CreatedAtAction(nameof(GetPayment), new { id = created.id }, created);
        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePayment(int id, Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _paymentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            payment.id = id;
            await _paymentRepository.UpdateAsync(payment);
            return NoContent();
        }

        // PATCH: api/Payment/5/status/Completed
        [HttpPatch("{id}/status/{status}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var existing = await _paymentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _paymentRepository.UpdateStatusAsync(id, status);
            return NoContent();
        }

        // PATCH: api/Payment/5/transaction/ABC123
        [HttpPatch("{id}/transaction/{transactionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTransaction(int id, string transactionId)
        {
            var existing = await _paymentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _paymentRepository.UpdateTransactionIdAsync(id, transactionId);
            return NoContent();
        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var existing = await _paymentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _paymentRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}