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

        public async Task<IEnumerable<Shipping>> GetShippingByCarrier(string carrier)
        {
            return await _dbContext.Shipping
                .Where(s => s.Carrier == carrier)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shipping>> GetShippingByStatus(string status)
        {
            return await _dbContext.Shipping
                .Where(s => s.Status == status)
                .ToListAsync();
        }

        public async Task<Shipping> GetShippingForOrder(int orderId)
        {
            return await _dbContext.Shipping
                .FirstOrDefaultAsync(s => s.OrderId == orderId);
        }

        public async Task UpdateDelivery(int shippingId, DateTime estimatedDelivery)
        {
            await _dbContext.Shipping
                .Where(s => s.Id == shippingId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.EstimatedDelivery, estimatedDelivery));
        }

        public async Task UpdateShippingStatus(int shippingId, string status)
        {
            await _dbContext.Shipping
                .Where(s => s.Id == shippingId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, status));
        }

        public async Task UpdateTrackNumber(int shippingId, string trackingNumber)
        {
            await _dbContext.Shipping
                .Where(s => s.Id == shippingId)
                .ExecuteUpdateAsync(s => s.SetProperty(tn => tn.TrackingNumber, trackingNumber));
        }
    }
}
