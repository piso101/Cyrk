using BackEndCyrk.Data;
using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _dbContext.Products
                .Where(p => p.category_id == categoryId)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            try
            {
                await base.UpdateAsync(product);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await DeleteByIdAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            try
            {
                await _dbContext.Products
                    .Where(p => p.id == productId)
                    .ExecuteUpdateAsync(p => p.SetProperty(x => x.stock, quantity));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm)
        {
            return await _dbContext.Products
                .Where(p => p.name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await base.ExistsAsync(p => p.id == id);
        }
    }
}