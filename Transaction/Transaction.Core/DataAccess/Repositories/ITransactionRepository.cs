using Transaction.Core.Models;

namespace Transaction.Core.DataAccess.Repositories;

public interface ITransactionRepository
{
    Task<TransactionEntity> GetByIdAsync(int id);
    
    Task<List<TransactionEntity>> GetAllAsync();
    
    Task<List<TransactionEntity>> GetByStatusAsync(TransactionStatus status);
    
    Task<List<TransactionEntity>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
    
    Task AddAsync(TransactionEntity transaction);
    
    Task UpdateAsync(TransactionEntity transaction);
    
    Task DeleteByIdAsync(int id);
}