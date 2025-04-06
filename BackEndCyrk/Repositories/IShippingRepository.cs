using BackEndCyrk.Models;

namespace BackEndCyrk.Repositories
{
    public interface IShippingRepository : IBaseRepository
    {
        Task<IEnumerable<Shipping>> GetShippingByStatus(string status);
        Task<Shipping> GetShippingForOrder(int orderId);
        Task UpdateShippingStatus(int shippingId, string status);
        Task UpdateTrackNumber(int shippingId, string trackingNumber);
        Task UpdateDelivery(int shippingId, DateTime estimatedDelivery);
        Task<IEnumerable<Shipping>> GetShippingByCarrier(string carrier);
    }
}
