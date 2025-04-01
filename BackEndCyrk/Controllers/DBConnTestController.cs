using BackEndCyrk.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DBConnTestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DBConnTestController> _logger;

        public DBConnTestController(ApplicationDbContext context, ILogger<DBConnTestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // Próba połączenia z bazą danych
                bool canConnect = await _context.Database.CanConnectAsync();

                if (canConnect)
                {
                    return Ok(new { message = "Połączenie z bazą danych ustanowione pomyślnie!" });
                }
                else
                {
                    return BadRequest(new { message = "Nie można połączyć się z bazą danych." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas testowania połączenia z bazą danych");
                return StatusCode(500, new { message = $"Błąd połączenia: {ex.Message}" });
            }
        }
    }
}