using BackEndCyrk.Models;
using static BackEndCyrk.Repositories.IBaseRepository;

namespace BackEndCyrk.Repositories
{
    public interface IShippingRateRepository : IBaseRepository<ShippingRate>
    {
        Task<IEnumerable<ShippingRate>> GetByCarrier(string carrier);
        Task<IEnumerable<ShippingRate>> GetByPackageType(string packageType);
        Task<ShippingRate> GetByCarrierAndPackageType(string carrier, string packageType);
        Task<decimal> GetLowestRate();
        Task<decimal> GetHighestRate();
        Task<IEnumerable<ShippingRate>> GetByPriceRange(decimal minPrice, decimal maxPrice);
    }
}
