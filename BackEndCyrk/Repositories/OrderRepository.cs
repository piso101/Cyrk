using BackEndCyrk.Data;
using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Repositories
{
    /// <summary>
    /// Repository implementation for Order entity
    /// </summary>
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Update order status
        /// </summary>
        public async Task UpdateStatus(int orderId, string status)
        {
            await _dbContext.Set<Order>()
                .Where(o => o.id == orderId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.status, status));
        }

        /// <summary>
        /// Get orders by user ID
        /// </summary>
        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _dbContext.Orders
                .Where(o => o.user_id == userId)
                .OrderByDescending(o => o.created_at)
                .ToListAsync();
        }

        /// <summary>
        /// Get orders by status
        /// </summary>
        public async Task<IEnumerable<Order>> GetByStatusAsync(string status)
        {
            return await _dbContext.Orders
                .Where(o => o.status == status)
                .OrderByDescending(o => o.created_at)
                .ToListAsync();
        }

        /// <summary>
        /// Get order with all related data (User, OrderItems, Payment, Shipping)
        /// </summary>
        public async Task<Order?> GetWithDetailsAsync(int id)
        {
            return await _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .Include(o => o.Shipping)
                .FirstOrDefaultAsync(o => o.id == id);
        }

        /// <summary>
        /// Calculate total revenue from all completed orders
        /// </summary>
        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _dbContext.Orders
                .Where(o => o.status == "Completed")
                .SumAsync(o => o.total_price);
        }

        /// <summary>
        /// Get orders within date range
        /// </summary>
        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Orders
                .Where(o => o.created_at >= startDate && o.created_at <= endDate)
                .OrderByDescending(o => o.created_at)
                .ToListAsync();
        }
    }
}