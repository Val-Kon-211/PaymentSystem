using Microsoft.EntityFrameworkCore;
using Transaction.Core.Models;

namespace Transaction.Core.DataAccess.Repositories;

public class OutboxMessageRepository: IOutboxMessageRepository
{
    private readonly TransactionDbContext _context;
    
    public async Task AddAsync(OutboxMessage message)
    {
        _context.OutboxMessages.Add(message);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<OutboxMessage>> GetAll()
    {
        return await _context.OutboxMessages.ToListAsync();
    }

    public async Task<List<OutboxMessage>> GetUnprocessed()
    {
        return await _context.OutboxMessages
            .Where(m => !m.Processed)
            .ToListAsync();
    }

    public async Task<OutboxMessage> GetByIdAsync(int id)
    {
        return await _context.OutboxMessages.FindAsync(id);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}