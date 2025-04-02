using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models
{
    public class Payment : BaseEntity
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [StringLength(255)]
        public string TransactionId { get; set; }

        // Relacja do tabeli Orders
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
