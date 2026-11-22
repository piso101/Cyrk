namespace BackEndCyrk.DTOs
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DTO używane podczas logowania użytkownika
    /// </summary>
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public required string email { get; set; }
        [Required]
        public required string password { get; set; }
    }

    /// <summary>
    /// DTO używane podczas rejestracji nowego użytkownika
    /// </summary>
    public class UserRegisterDto
    {
        [Required]
        [StringLength(255)]
        public required string name { get; set; }
        [Required]
        [StringLength(255)]
        public required string surname { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public required string email { get; set; }
        [Required]
        [MinLength(6)]
        public required string password { get; set; }
        [Required]
        [Compare("password")]
        public required string confirm_password { get; set; }
        public string? address { get; set; }
        [StringLength(20)]
        public string? phone { get; set; }
    }

    /// <summary>
    /// DTO do odczytu danych użytkownika
    /// </summary>
    public class UserReadDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string surname { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string? address { get; set; }
        public string? phone { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

    /// <summary>
    /// DTO do aktualizacji danych użytkownika
    /// </summary>
    public class UserUpdateDto
    {
        [StringLength(255)]
        public string? name { get; set; }
        [StringLength(255)]
        public string? surname { get; set; }
        [EmailAddress]
        [StringLength(255)]
        public string? email { get; set; }
        public string? address { get; set; }
        [StringLength(20)]
        public string? phone { get; set; }
    }

    /// <summary>
    /// DTO zwracane po pomyślnym uwierzytelnieniu użytkownika
    /// </summary>
    public class AuthResultDto
    {
        public string token { get; set; } = null!;
        public DateTime expires_at { get; set; }
        public UserReadDto user { get; set; } = null!;
    }
}