using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEndCyrk.Repositories
{
    /// <summary>
    /// Implementacja repozytorium stanów magazynowych
    /// </summary>
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(DbContext context) : base(context)
        {
        }

        public async Task<Inventory> GetByProductIdAsync(int productId)
        {
            return await GetSingleAsync(i => i.product_id == productId);
        }

        public async Task UpdateQuantityAsync(int inventoryId, int quantity)
        {
            var inventory = await GetByIdAsync(inventoryId);
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory with id {inventoryId} not found.");

            inventory.quantity = quantity;
            inventory.last_updated = DateTime.UtcNow;
            await UpdateAsync(inventory);
        }

        public async Task UpdateLastUpdatedAsync(int inventoryId, DateTime lastUpdated)
        {
            var inventory = await GetByIdAsync(inventoryId);
            if (inventory == null)
                throw new KeyNotFoundException($"Inventory with id {inventoryId} not found.");

            inventory.last_updated = lastUpdated;
            await UpdateAsync(inventory);
        }

        public async Task<IEnumerable<Inventory>> GetLowStockAsync(int threshold)
        {
            return await GetAsync(i => i.quantity <= threshold);
        }

        public async Task<IEnumerable<Inventory>> GetUpdatedAfterAsync(DateTime since)
        {
            return await GetAsync(i => i.last_updated >= since);
        }
    }
}
