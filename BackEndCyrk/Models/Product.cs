using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndCyrk.Models;

public class Product : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
        
    public string Description { get; set; }
        
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
        
    // Klucz obcy
    public int? CategoryId { get; set; }
        
    // Relacja z Categories
    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }
        
    [Required]
    public int Stock { get; set; } = 0;
}