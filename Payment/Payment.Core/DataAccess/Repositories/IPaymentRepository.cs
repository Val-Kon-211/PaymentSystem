using Payment.Core.Models;

namespace Payment.Core.DataAccess.Repositories;

public interface IPaymentRepository
{
    Task<PaymentEntity> GetByIdAsync(int id);
    Task<List<PaymentEntity>> GetAllAsync();
    Task AddAsync(PaymentEntity payment);
    Task UpdateAsync(PaymentEntity payment);
    Task DeleteByIdAsync(int id);
}