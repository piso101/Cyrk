using System.ComponentModel.DataAnnotations;

namespace BackEndCyrk.Models;

public class Category : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
        
    public string Description { get; set; }
        
    // Relacja jeden-do-wielu z Products
    public virtual ICollection<Product> Products { get; set; }
}
