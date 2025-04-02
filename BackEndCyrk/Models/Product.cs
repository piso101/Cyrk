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
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        public int Stock { get; set; } = 0;

        // Navigation properties
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public Inventory Inventory { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}