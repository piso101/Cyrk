using BackEndCyrk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BackEndCyrk.Repositories.IBaseRepository;

namespace BackEndCyrk.Repositories
{
    /// <summary>
    /// Repozytorium do obs³ugi p³atnoœci
    /// </summary>
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetByStatusAsync(string status);
        Task<Payment> GetByOrderIdAsync(int orderId);
        Task UpdateStatusAsync(int paymentId, string status);
        Task UpdateTransactionIdAsync(int paymentId, string transactionId);
        Task<IEnumerable<Payment>> GetByPaymentMethodAsync(string method);
    }
}