using BackEndCyrk.Models;
using static BackEndCyrk.Repositories.IBaseRepository;

namespace BackEndCyrk.Repositories
{
    /// <summary>
    /// Interface for Order repository operations
    /// </summary>
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task UpdateStatus(int orderId, string status);
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetByStatusAsync(string status);
        Task<Order?> GetWithDetailsAsync(int id);
        Task<decimal> GetTotalRevenueAsync();
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}