using Payment.Core.Models;

namespace Payment.Service.Interfaces;

public interface IOutboxService
{
    Task AddMessageAsync(string messageType, string payload);

    Task<List<OutboxMessage>> GetUnprocessedMessagesAsync();

    Task MarkAsProcessedAsync(int messageId);
}