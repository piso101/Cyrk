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
            return await _dbContext.ShippingRates.MaxAsync(sr => sr.price);
        }

        public async Task<decimal> GetLowestRate()
        {
            return await _dbContext.ShippingRates.MinAsync(sr => sr.price);
        }

        /// <summary>
        /// Moze zwracac null
        /// </summary>
        /// <param name="carrier"></param>
        /// <param name="packageType"></param>
        /// <returns></returns>
        public async Task<ShippingRate> GetByCarrierAndPackageType(string carrier, string packageType)
        {
            return await _dbContext.ShippingRates.Where(sr => sr.carrier == carrier && sr.package_type == packageType)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ShippingRate>> GetByCarrier(string carrier)
        {
            return await _dbContext.ShippingRates.Where(sr => sr.carrier == carrier).ToListAsync();
        }

        public async Task<IEnumerable<ShippingRate>> GetByPackageType(string packageType)
        {
            return await _dbContext.ShippingRates.Where(sr => sr.package_type == packageType)
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
        public async Task<IEnumerable<ShippingRate>> GetByPriceRange(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0)
            {
                throw new ArgumentException("Price range cannot be negative.");
            }
            if (minPrice > maxPrice)
            {
                throw new ArgumentException("Minimum price cannot be greater than maximum price.");
            }

            return await _dbContext.ShippingRates.Where(sr => sr.price >= minPrice && sr.price <= maxPrice)
                .ToListAsync();
        }
    }
}
