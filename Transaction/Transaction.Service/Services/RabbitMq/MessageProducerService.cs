using Microsoft.Extensions.Logging;
using Transaction.Service.Interfaces;

namespace Transaction.Service.Services.RabbitMq;

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
        _logger.LogInformation($"Сообщение сохранено в Transaction.Outbox: {messagePayload}");
    }
}