using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BackEndCyrk.Repositories;

/// <summary>
/// Implementacja generycznego bazowego repozytorium dla operacji CRUD
/// </summary>
/// <typeparam name="T">Typ encji</typeparam>
public class BaseRepository<T> : IBaseRepository.IBaseRepository<T> where T : class {
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext context) {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter) {
        return await _dbSet.Where(filter).ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(int id) {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter) {
        return await _dbSet.FirstOrDefaultAsync(filter);
    }

    public virtual async Task<T> AddAsync(T entity) {
        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities) {
        await _dbSet.AddRangeAsync(entities);
        await SaveChangesAsync();
        return entities;
    }

    public virtual async Task UpdateAsync(T entity) {
        _context.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity) {
        _dbSet.Remove(entity);
        await SaveChangesAsync();
    }

    public virtual async Task DeleteByIdAsync(int id) {
        var entity = await GetByIdAsync(id);
        if (entity != null) {
            await DeleteAsync(entity);
        }
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities) {
        _dbSet.RemoveRange(entities);
        await SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter) {
        return await _dbSet.AnyAsync(filter);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> filter = null) {
        if (filter == null) {
            return await _dbSet.CountAsync();
        }

        return await _dbSet.CountAsync(filter);
    }

    public virtual async Task<int> SaveChangesAsync() {
        return await _context.SaveChangesAsync();
    }
}