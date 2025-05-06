using BackEndCyrk.Models;
using static BackEndCyrk.Repositories.IBaseRepository;

namespace BackEndCyrk.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task UpdateStatus(int orderId, string status);
    }
}
