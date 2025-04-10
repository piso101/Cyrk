using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public string description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }

        public int? category_id { get; set; }

        [Required]
        public int stock { get; set; } = 0;

        // Navigation properties
        [ForeignKey("category_id")]
        public virtual Category Category { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}