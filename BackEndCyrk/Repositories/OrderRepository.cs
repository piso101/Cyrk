using BackEndCyrk.Data;
using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task UpdateStatus(int orderId, string status)
        {
            await _dbContext.Set<Order>()
                .Where(o => o.id == orderId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.status, status));
        }
    }
}
