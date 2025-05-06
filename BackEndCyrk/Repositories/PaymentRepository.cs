using BackEndCyrk.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEndCyrk.Repositories
{
    /// <summary>
    /// Implementacja repozytorium p³atnoœci
    /// </summary>
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(string status)
        {
            return await GetAsync(p => p.status == status);
        }

        public async Task<Payment> GetByOrderIdAsync(int orderId)
        {
            return await GetSingleAsync(p => p.order_id == orderId);
        }

        public async Task UpdateStatusAsync(int paymentId, string status)
        {
            var payment = await GetByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with id {paymentId} not found.");

            payment.status = status;
            await UpdateAsync(payment);
        }

        public async Task UpdateTransactionIdAsync(int paymentId, string transactionId)
        {
            var payment = await GetByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with id {paymentId} not found.");

            payment.transaction_id = transactionId;
            await UpdateAsync(payment);
        }

        public async Task<IEnumerable<Payment>> GetByPaymentMethodAsync(string method)
        {
            return await GetAsync(p => p.payment_method == method);
        }
    }
}
