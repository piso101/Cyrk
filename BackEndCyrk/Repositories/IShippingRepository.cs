using BackEndCyrk.Models;
using static BackEndCyrk.Repositories.IBaseRepository;

namespace BackEndCyrk.Repositories
{
    public interface IShippingRepository : IBaseRepository<Shipping>
    {
        Task<IEnumerable<Shipping>> GetByStatus(string status);
        Task<Shipping> GetByOrderId(int orderId);
        Task UpdateStatus(int shippingId, string status);
        Task UpdateTrackNumber(int shippingId, string trackingNumber);
        Task UpdateDelivery(int shippingId, DateTime estimatedDelivery);
        Task<IEnumerable<Shipping>> GetByCarrier(string carrier);
    }
}
