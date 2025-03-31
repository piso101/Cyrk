using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models;

public class Shipping : BaseEntity
{
    [Required]
    public int OrderId { get; set; }

    [StringLength(255)]
    public string TrackingNumber { get; set; }

    [StringLength(100)]
    public string Carrier { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Pending";

    [Column(TypeName = "Date")]
    public DateTime? EstimatedDelivery { get; set; }

    [Required]
    public string Address { get; set; }

    // Relacje
    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; }
}