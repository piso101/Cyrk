using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.DTOs
{
    /// <summary>
    /// DTO do tworzenia nowego zamówienia
    /// </summary>
    public class OrderCreateDto
    {
        [Required]
        public int user_id { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal total_price { get; set; }

        [StringLength(50)]
        public string status { get; set; } = "Pending";
    }

    /// <summary>
    /// DTO do odczytu danych zamówienia
    /// </summary>
    public class OrderReadDto
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public decimal total_price { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
    }

    /// <summary>
    /// DTO do aktualizacji zamówienia
    /// </summary>
    public class OrderUpdateDto
    {
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal total_price { get; set; }

        [StringLength(50)]
        public string status { get; set; }
    }
}
