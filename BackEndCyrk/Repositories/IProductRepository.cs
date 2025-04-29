using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndCyrk.Models;

namespace BackEndCyrk.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        Task<Product> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        
        Task<bool> UpdateStockAsync(int productId, int quantity);
        
        Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm);
        
        Task<bool> ExistsAsync(int id);
    }
}