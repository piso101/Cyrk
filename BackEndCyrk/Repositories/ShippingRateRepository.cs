using BackEndCyrk.Data;
using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BackEndCyrk.Repositories
{
    public class ShippingRateRepository : BaseRepository<ShippingRate>, IShippingRateRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShippingRateRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<decimal> GetHighestRate()
        {
            return await _dbContext.ShippingRates.MaxAsync(sr => sr.Price);
        }

        public async Task<decimal> GetLowestRate()
        {
            return await _dbContext.ShippingRates.MinAsync(sr => sr.Price);
        }

        /// <summary>
        /// Moze zwracac null
        /// </summary>
        /// <param name="carrier"></param>
        /// <param name="packageType"></param>
        /// <returns></returns>
        public async Task<ShippingRate> GetRateByCarrierAndPackageType(string carrier, string packageType)
        {
            return await _dbContext.ShippingRates.Where(sr => sr.Carrier == carrier && sr.PackageType == packageType)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ShippingRate>> GetRatesByCarrier(string carrier)
        {
            return await _dbContext.ShippingRates.Where(sr => sr.Carrier == carrier).ToListAsync();
        }

        public async Task<IEnumerable<ShippingRate>> GetRatesByPackageType(string packageType)
        {
            return await _dbContext.ShippingRates.Where(sr => sr.PackageType == packageType)
                .ToListAsync();
        }

        /// <summary>
        /// minPrice i maxPrice musza byc >= 0,
        /// minPrice musi być <= maxPrice
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<ShippingRate>> GetRatesByPriceRange(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0)
            {
                throw new ArgumentException("Price range cannot be negative.");
            }
            if (minPrice > maxPrice)
            {
                throw new ArgumentException("Minimum price cannot be greater than maximum price.");
            }

            return await _dbContext.ShippingRates.Where(sr => sr.Price >= minPrice && sr.Price <= maxPrice)
                .ToListAsync();
        }
    }
}
