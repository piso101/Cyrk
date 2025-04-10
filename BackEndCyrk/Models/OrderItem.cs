using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models
{
    public class OrderItem : BaseEntity
    {
        public int order_id { get; set; }

        public int product_id { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }

        // Navigation properties
        [ForeignKey("order_id")]
        public virtual Order Order { get; set; }

        [ForeignKey("product_id")]
        public virtual Product Product { get; set; }
    }
}