using Microsoft.AspNetCore.Mvc;
using BackEndCyrk.DTOs;
using BackEndCyrk.Models;
using BackEndCyrk.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BackEndCyrk.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za logowanie, rejestrację oraz uwierzytelnianie użytkowników
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IUserRepository userRepository, IConfiguration configuration, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// Metoda testowa do debugowania bazy danych
        /// </summary>
        [HttpGet("debug-db")]
        public async Task<IActionResult> DebugDatabase()
        {
            try
            {
                Console.WriteLine("=== DEBUG DATABASE START ===");

                // Test połączenia
                var canConnect = await _context.Database.CanConnectAsync();
                Console.WriteLine($"Can connect to DB: {canConnect}");

                // Sprawdź liczbę użytkowników
                var userCount = await _context.Users.CountAsync();
                Console.WriteLine($"Total users count: {userCount}");

                // Pobierz wszystkich użytkowników
                var allUsers = await _context.Users.ToListAsync();
                Console.WriteLine($"Users in database:");
                foreach (var u in allUsers)
                {
                    Console.WriteLine($"- ID: {u.id}, Email: {u.email}, Name: {u.name}");
                }

                // Sprawdź nazwy kolumn
                var columns = await _context.Database.SqlQueryRaw<string>("SELECT column_name FROM information_schema.columns WHERE table_name = 'users'").ToListAsync();
                Console.WriteLine($"Columns in users table: {string.Join(", ", columns)}");

                Console.WriteLine("=== DEBUG DATABASE END ===");

                return Ok(new
                {
                    canConnect = canConnect,
                    userCount = userCount,
                    users = allUsers.Select(u => new { u.id, u.email, u.name }),
                    columns = columns
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database debug error: {ex.Message}");
                return BadRequest($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Test konkretnego emaila
        /// </summary>
        [HttpGet("test-email/{email}")]
        public async Task<IActionResult> TestEmail(string email)
        {
            try
            {
                Console.WriteLine($"=== TESTING EMAIL: {email} ===");

                // Test przez repository
                var userFromRepo = await _userRepository.GetByEmailAsync(email);
                Console.WriteLine($"User from repository: {userFromRepo?.email ?? "NULL"}");

                // Test bezpośrednio przez EF
                var userFromEF = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
                Console.WriteLine($"User from EF direct: {userFromEF?.email ?? "NULL"}");

                // Test z ignorecase
                var userIgnoreCase = await _context.Users.FirstOrDefaultAsync(u => u.email.ToLower() == email.ToLower());
                Console.WriteLine($"User ignore case: {userIgnoreCase?.email ?? "NULL"}");

                return Ok(new
                {
                    inputEmail = email,
                    foundViaRepository = userFromRepo != null,
                    foundViaEF = userFromEF != null,
                    foundIgnoreCase = userIgnoreCase != null,
                    userFromRepo = userFromRepo,
                    userFromEF = userFromEF
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test email error: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Loguje użytkownika na podstawie adresu e-mail i hasła
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                Console.WriteLine("=== LOGIN ATTEMPT START ===");
                Console.WriteLine($"Input email: '{loginDto.email}'");
                Console.WriteLine($"Input password length: {loginDto.password?.Length ?? 0}");

                // Sprawdź czy baza jest dostępna
                var canConnect = await _context.Database.CanConnectAsync();
                Console.WriteLine($"Database connection: {canConnect}");

                if (!canConnect)
                {
                    Console.WriteLine("❌ Cannot connect to database");
                    return StatusCode(500, "Database connection failed");
                }

                // Sprawdź liczbę użytkowników
                var totalUsers = await _context.Users.CountAsync();
                Console.WriteLine($"Total users in database: {totalUsers}");

                // Test przez repository
                Console.WriteLine("Trying repository method...");
                var userFromRepo = await _userRepository.GetByEmailAsync(loginDto.email);
                Console.WriteLine($"User from repository: {userFromRepo?.email ?? "NULL"}");

                // Test bezpośrednio przez EF
                Console.WriteLine("Trying direct EF method...");
                var userFromEF = await _context.Users
                    .FirstOrDefaultAsync(u => u.email == loginDto.email);
                Console.WriteLine($"User from EF: {userFromEF?.email ?? "NULL"}");

                // Test case insensitive
                Console.WriteLine("Trying case insensitive...");
                var userIgnoreCase = await _context.Users
                    .FirstOrDefaultAsync(u => u.email.ToLower() == loginDto.email.ToLower());
                Console.WriteLine($"User ignore case: {userIgnoreCase?.email ?? "NULL"}");

                // Wybierz użytkownika do logowania
                var user = userFromEF ?? userIgnoreCase;

                if (user == null)
                {
                    Console.WriteLine($"❌ User not found for email: {loginDto.email}");

                    // Pokaż wszystkich użytkowników dla debugowania
                    var allEmails = await _context.Users.Select(u => u.email).ToListAsync();
                    Console.WriteLine($"Available emails in database: {string.Join(", ", allEmails)}");

                    return Unauthorized($"Nie istnieje użytkownik: {loginDto.email}");
                }

                Console.WriteLine($"✅ User found: {user.email}");

                // Sprawdź hasło
                Console.WriteLine("Verifying password...");
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.password, user.password_hash);
                Console.WriteLine($"Password valid: {isPasswordValid}");

                if (!isPasswordValid)
                {
                    Console.WriteLine("❌ Invalid password");
                    return Unauthorized("Nieprawidłowe hasło.");
                }

                Console.WriteLine("✅ Password valid, generating token...");

                var token = GenerateJwtToken(user.id, user.email);
                var expiresAt = DateTime.UtcNow.AddHours(24);

                var result = new AuthResultDto
                {
                    token = token,
                    expires_at = expiresAt,
                    user = new UserReadDto
                    {
                        id = user.id,
                        name = user.name,
                        surname = user.surname,
                        email = user.email,
                        address = user.address,
                        phone = user.phone
                    }
                };

                Console.WriteLine("✅ Login successful");
                Console.WriteLine("=== LOGIN ATTEMPT END ===");

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Login error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"Login error: {ex.Message}");
            }
        }

        /// <summary>
        /// Zwraca dane bieżącego zalogowanego użytkownika (na podstawie tokena JWT)
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            return Ok(new
            {
                id = userId,
                email = email,
                message = "Token is valid."
            });
        }

        /// <summary>
        /// Rejestruje nowego użytkownika w systemie
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                Console.WriteLine($"=== REGISTRATION ATTEMPT ===");
                Console.WriteLine($"Email: {registerDto.email}");
                Console.WriteLine($"Name: {registerDto.name}");

                // Sprawdź czy użytkownik już istnieje
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.email.ToLower() == registerDto.email.ToLower());

                if (existingUser != null)
                {
                    Console.WriteLine($"❌ User already exists: {existingUser.email}");
                    return BadRequest("User with this email already exists.");
                }

                // Sprawdź czy hasła się zgadzają
                if (registerDto.password != registerDto.confirm_password)
                {
                    Console.WriteLine("❌ Passwords do not match");
                    return BadRequest("Passwords do not match.");
                }

                // Utwórz nowego użytkownika
                var user = new User
                {
                    name = registerDto.name,
                    surname = registerDto.surname,
                    email = registerDto.email,
                    password_hash = BCrypt.Net.BCrypt.HashPassword(registerDto.password),
                    address = registerDto.address,
                    phone = registerDto.phone
                };

                var addedUser = await _userRepository.AddAsync(user);
                Console.WriteLine($"✅ User registered successfully: {addedUser.email}");

                return Ok("Registration successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Registration error: {ex.Message}");
                return StatusCode(500, $"Registration error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(int userId, string email)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "SuperTajnyKluczMinimum32ZnakiDlaJWT123456789";
            var issuer = jwtSettings["Issuer"] ?? "BackEndCyrk";
            var audience = jwtSettings["Audience"] ?? "BackEndCyrkUsers";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}