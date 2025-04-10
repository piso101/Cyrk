using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models
{
    public class Payment : BaseEntity
    {
        [Required]
        public int order_id { get; set; }

        [Required]
        [StringLength(50)]
        public string payment_method { get; set; }

        [StringLength(50)]
        public string status { get; set; } = "Pending";

        [StringLength(255)]
        public string transaction_id { get; set; }

        // Relacja do tabeli Orders
        [ForeignKey("order_id")]
        public virtual Order order { get; set; }
    }
}
