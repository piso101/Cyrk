using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEndCyrk.Models;

public class Category : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string name { get; set; }
        
    public string description { get; set; }
        
    // Relacja jeden-do-wielu z Products
   // [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
