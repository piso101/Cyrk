using BackEndCyrk.Data;
using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BackEndCyrk.Repositories
{
    public class ShippingRepository : BaseRepository<Shipping>, IShippingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShippingRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Shipping>> GetByCarrier(string carrier)
        {
            return await _dbContext.Shipping
                .Where(s => s.carrier == carrier)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shipping>> GetByStatus(string status)
        {
            return await _dbContext.Shipping
                .Where(s => s.status == status)
                .ToListAsync();
        }

        public async Task<Shipping> GetByOrderId(int orderId)
        {
            return await _dbContext.Shipping
                .FirstOrDefaultAsync(s => s.order_id == orderId);
        }

        public async Task UpdateDelivery(int shippingId, DateTime estimatedDelivery)
        {
            await _dbContext.Shipping
                .Where(s => s.id == shippingId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.estimated_delivery, estimatedDelivery));
        }

        public async Task UpdateStatus(int shippingId, string status)
        {
            await _dbContext.Shipping
                .Where(s => s.id == shippingId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.status, status));
        }

        public async Task UpdateTrackNumber(int shippingId, string trackingNumber)
        {
            await _dbContext.Shipping
                .Where(s => s.id == shippingId)
                .ExecuteUpdateAsync(s => s.SetProperty(tn => tn.tracking_number, trackingNumber));
        }
    }
}
