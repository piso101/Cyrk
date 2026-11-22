using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.DTOs
{
    /// <summary>
    /// DTO do tworzenia nowej pozycji zamówienia
    /// </summary>
    public class OrderItemCreateDto
    {
        [Required]
        public int order_id { get; set; }

        [Required]
        public int product_id { get; set; }

        [Required]
        public int quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }
    }

    /// <summary>
    /// DTO do odczytu danych pozycji zamówienia
    /// </summary>
    public class OrderItemReadDto
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public DateTime created_at { get; set; }
    }

    /// <summary>
    /// DTO do aktualizacji pozycji zamówienia
    /// </summary>
    public class OrderItemUpdateDto
    {
        [Required]
        public int quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }
    }
}
