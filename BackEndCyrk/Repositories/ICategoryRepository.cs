using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndCyrk.Models;

namespace BackEndCyrk.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<Category> GetWithProductsAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Category>> SearchByNameAsync(string searchTerm);
    }
}