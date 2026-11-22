using BackEndCyrk.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BackEndCyrk.Repositories.IBaseRepository;

namespace BackEndCyrk.Repositories
{
    /// <summary>
    /// Repozytorium do obs³ugi stanów magazynowych
    /// </summary>
    public interface IInventoryRepository : IBaseRepository<Inventory>
    {
        Task<Inventory> GetByProductIdAsync(int productId);
        Task UpdateQuantityAsync(int inventoryId, int quantity);
        Task UpdateLastUpdatedAsync(int inventoryId, DateTime lastUpdated);
        Task<IEnumerable<Inventory>> GetLowStockAsync(int threshold);
        Task<IEnumerable<Inventory>> GetUpdatedAfterAsync(DateTime since);
    }
}