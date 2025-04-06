using BackEndCyrk.Models;
using static BackEndCyrk.Repositories.IBaseRepository;

namespace BackEndCyrk.Repositories
{
    public interface IShippingRateRepository : IBaseRepository<ShippingRate>
    {
        Task<IEnumerable<ShippingRate>> GetRatesByCarrier(string carrier);
        Task<IEnumerable<ShippingRate>> GetRatesByPackageType(string packageType);
        Task<ShippingRate> GetRateByCarrierAndPackageType(string carrier, string packageType);
        Task<decimal> GetLowestRate();
        Task<decimal> GetHighestRate();
        Task<IEnumerable<ShippingRate>> GetRatesByPriceRange(decimal minPrice, decimal maxPrice);
    }
}
