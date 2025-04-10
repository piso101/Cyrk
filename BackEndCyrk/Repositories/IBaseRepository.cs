using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Repositories;

public interface IBaseRepository
{
    /// <summary>
    /// Generyczny interfejs bazowego repozytorium dla operacji CRUD
    /// </summary>
    /// <typeparam name="T">Typ encji</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        // Metody do pobierania danych
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(int id);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> filter);
        
        // Metody do modyfikacji danych
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteByIdAsync(int id);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        
        // Metody pomocnicze
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
        Task<int> SaveChangesAsync();
    }
}