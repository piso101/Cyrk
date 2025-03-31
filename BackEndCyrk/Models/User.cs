using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models;

public class User : BaseEntity
{

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    [StringLength(255)]
    public string Surname { get; set; }

    [Required]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public string Address { get; set; }

    [StringLength(20)]
    public string Phone { get; set; }

    // Relacje
    public virtual ICollection<Order> Orders { get; set; }
}