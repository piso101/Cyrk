using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackEndCyrk.Models;

public class ShippingRate : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string carrier { get; set; }

    [Required]
    [StringLength(100)]
    public string package_type { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal price { get; set; }
}