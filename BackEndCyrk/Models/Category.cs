using System.ComponentModel.DataAnnotations;

namespace BackEndCyrk.Models;

public class Category : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string name { get; set; }
        
    public string description { get; set; }
        
    // Relacja jeden-do-wielu z Products
    public virtual ICollection<Product> Products { get; set; }
}
