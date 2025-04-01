using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Payment Payment { get; set; }

        public Shipping Shipping { get; set; }
    }
}