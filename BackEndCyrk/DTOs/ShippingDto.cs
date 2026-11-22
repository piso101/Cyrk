namespace BackEndCyrk.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// DTO dla tworzenia nowej przesyłki
    /// </summary>
    public class ShippingCreateDto
    {
        [Required]
        public int order_id { get; set; }

        [StringLength(255)]
        public string tracking_number { get; set; }

        [StringLength(100)]
        public string carrier { get; set; }

        [StringLength(50)]
        public string status { get; set; } = "Pending";

        public DateTime? estimated_delivery { get; set; }

        [Required]
        public string address { get; set; }
    }

    /// <summary>
    /// DTO dla odczytywania danych przesyłki
    /// </summary>
    public class ShippingReadDto
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public string tracking_number { get; set; }
        public string carrier { get; set; }
        public string status { get; set; }
        public DateTime? estimated_delivery { get; set; }
        public string address { get; set; }
        public DateTime created_at { get; set; }
    }

    /// <summary>
    /// DTO dla aktualizacji przesyłki
    /// </summary>
    public class ShippingUpdateDto
    {
        [StringLength(255)]
        public string tracking_number { get; set; }

        [StringLength(100)]
        public string carrier { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        public DateTime? estimated_delivery { get; set; }

        public string address { get; set; }
    }
}
