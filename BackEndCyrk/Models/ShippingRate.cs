using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackEndCyrk.Models;

public class ShippingRate : BaseEntity
{
    [Required]
    [StringLength(100)]
    public required string carrier { get; set; }

    [Required]
    [StringLength(100)]
    public required string package_type { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public required decimal price { get; set; }
}