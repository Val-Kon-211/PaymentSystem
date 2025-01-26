using Microsoft.EntityFrameworkCore;
using Transaction.Core.Models;

namespace Transaction.Core.DataAccess.Repositories;

public class TransactionRepository: ITransactionRepository
{
    private readonly TransactionDbContext _context;

    public TransactionRepository(TransactionDbContext context)
    {
        _context = context;
    }
    
    public async Task<TransactionEntity> GetByIdAsync(int id)
    {
        return await _context.Transactions.FindAsync(id);
    }
    
    public async Task<List<TransactionEntity>> GetAllAsync()
    {
        return await _context.Transactions.ToListAsync();
    }
    
    public async Task<List<TransactionEntity>> GetByStatusAsync(TransactionStatus status)
    {
        return await _context.Transactions
            .Where(t => t.Status == status)
            .ToListAsync();
    }
    
    public async Task<List<TransactionEntity>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Transactions
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .ToListAsync();
    }
    
    public async Task AddAsync(TransactionEntity transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(TransactionEntity transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteByIdAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}