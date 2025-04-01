using BackEndCyrk.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BackEndCyrk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DbDiagnosticController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbDiagnosticController> _logger;

        public DbDiagnosticController(IConfiguration configuration, ILogger<DbDiagnosticController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult RunDiagnostic()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    return BadRequest("Brak connection stringa w konfiguracji.");
                }

                string diagnosticResult = ConnectionTester.TestNpgsqlConnection(connectionString);

                // Logowanie wyniku do konsoli i plików logów
                _logger.LogInformation(diagnosticResult);
                Console.WriteLine(diagnosticResult);

                return Ok(new
                {
                    message = "Wykonano diagnostykę połączenia",
                    details = diagnosticResult
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas diagnostyki połączenia");
                return StatusCode(500, new { message = $"Błąd diagnostyki: {ex.Message}" });
            }
        }
    }
}