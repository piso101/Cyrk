using BackEndCyrk.Data;
using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Category> GetWithProductsAsync(int id)
        {
            return await _dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                await base.UpdateAsync(category);
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

        public async Task<bool> ExistsAsync(int id)
        {
            return await base.ExistsAsync(c => c.id == id);
        }

        public async Task<IEnumerable<Category>> SearchByNameAsync(string searchTerm)
        {
            return await _dbContext.Categories
                .Where(c => c.name.Contains(searchTerm))
                .ToListAsync();
        }
    }
}