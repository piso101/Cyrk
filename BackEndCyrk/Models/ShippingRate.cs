using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackEndCyrk.Models;

public class ShippingRate : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Carrier { get; set; }

    [Required]
    [StringLength(100)]
    public string PackageType { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
}