using Microsoft.Extensions.Logging;
using Payment.Service.Interfaces;

namespace Payment.Service.Services.RabbitMq;

public class MessageProducerService : IMessageProducerService
{
    private readonly IOutboxService _outboxService;
    private readonly ILogger<MessageProducerService> _logger;

    public MessageProducerService(IOutboxService outboxService, ILogger<MessageProducerService> logger)
    {
        _outboxService = outboxService;
        _logger = logger;
    }

    public async Task CreateMessageAsync(string messageType, string messagePayload)
    {
        await _outboxService.AddMessageAsync(messageType, messagePayload);
        _logger.LogInformation($"Сообщение сохранено в Payment.Outbox: {messagePayload}");
    }
}