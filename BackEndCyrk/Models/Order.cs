using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models
{
    public class Order : BaseEntity
    {
        public int user_id { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal total_price { get; set; }

        [StringLength(50)]
        public string status { get; set; } = "Pending";

        // Navigation properties
        [ForeignKey("user_id")]
        public virtual User User { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual Shipping Shipping { get; set; }
    }
}