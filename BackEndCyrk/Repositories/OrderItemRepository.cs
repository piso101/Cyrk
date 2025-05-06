using BackEndCyrk.Data;
using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndCyrk.Repositories
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderId(int orderId)
        {
            return await _dbContext.Set<OrderItem>()
                .Where(item => item.order_id == orderId)
                .ToListAsync();
        }
    }
}
