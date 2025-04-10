using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        public string surname { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password_hash { get; set; }

        public string address { get; set; }

        [StringLength(20)]
        public string phone { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
    }
}