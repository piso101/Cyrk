using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models
{
    public class Inventory : BaseEntity
    {
        [Required]
        public int product_id { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        public DateTime last_updated { get; set; } = DateTime.UtcNow;

        // Relacja do tabeli Products
        [ForeignKey("product_id")]
        public virtual Product Product { get; set; }
    }
}
