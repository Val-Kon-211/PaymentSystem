using Microsoft.Extensions.Logging;
using Payment.Core.DataAccess.Repositories;
using Payment.Core.Models;
using Payment.Service.Interfaces;

namespace Payment.Service.Services.RabbitMq;

public class OutboxService : IOutboxService
{
    private readonly IOutboxMessageRepository _outboxMessageRepository;
    private readonly ILogger<OutboxService> _logger;
    private readonly PaymentPublisher _publisher;

    public OutboxService(
        IOutboxMessageRepository outboxMessageRepository,
        ILogger<OutboxService> logger, 
        PaymentPublisher publisher)
    {
        _outboxMessageRepository = outboxMessageRepository;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task AddMessageAsync(string messageType, string payload)
    {
        var message = new OutboxMessage
        {
            MessageType = messageType,
            Payload = payload,
            CreatedAt = DateTime.UtcNow,
            Processed = false
        };

        await _outboxMessageRepository.AddAsync(message);
    }

    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync()
    {
        return await _outboxMessageRepository.GetUnprocessed();
    }

    public async Task MarkAsProcessedAsync(int messageId)
    {
        var message = await _outboxMessageRepository.GetByIdAsync(messageId);
        if (message != null)
        {
            message.Processed = true;
            await _outboxMessageRepository.SaveChangesAsync();
        }
    }
}