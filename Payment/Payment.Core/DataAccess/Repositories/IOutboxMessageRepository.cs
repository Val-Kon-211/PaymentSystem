using Payment.Core.Models;

namespace Payment.Core.DataAccess.Repositories;

public interface IOutboxMessageRepository
{
    Task AddAsync(OutboxMessage message);

    Task<OutboxMessage> GetByIdAsync(int id);
    
    Task<List<OutboxMessage>> GetAll();

    Task<List<OutboxMessage>> GetUnprocessed();

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}