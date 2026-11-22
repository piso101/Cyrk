namespace BackEndCyrk.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// DTO dla tworzenia nowej przesyłki
    /// </summary>
    public class ShippingRateCreateDto
    {
        [Required]
        [StringLength(100)]
        public required string carrier { get; set; }

        [Required]
        [StringLength(100)]
        public required string package_type { get; set; }

        [Required]
        public required decimal price { get; set; }
    }

    /// <summary>
    /// DTO dla odczytywania danych przesyłki
    /// </summary>
    public class ShippingRateReadDto
    {
        public int id { get; set; }
        public string carrier { get; set; }
        public string package_type { get; set; }
        public decimal price { get; set; }
    }

    /// <summary>
    /// DTO dla aktualizacji przesyłki
    /// </summary>
    public class ShippingRateUpdateDto
    {
        [StringLength(100)]
        public required string carrier { get; set; }

        [StringLength(100)]
        public required string package_type { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public required decimal price { get; set; }
    }
}
