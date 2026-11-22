using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models;

public class Shipping : BaseEntity
{
    [Required]
    public required int order_id { get; set; }

    [StringLength(255)]
    public string tracking_number { get; set; }

    [StringLength(100)]
    public string carrier { get; set; }

    [StringLength(50)]
    public string status { get; set; } = "Pending";

    [Column(TypeName = "Date")]
    public DateTime? estimated_delivery { get; set; }

    [Required]
    public required string address { get; set; }

    // Relacje
    [ForeignKey("order_id")]
    public virtual Order Order { get; set; }
}