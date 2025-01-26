using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Payment.Service.Interfaces;
using RabbitMQ.Client;

namespace Payment.Service.Services.RabbitMq;

public class PaymentPublisher : BackgroundService
{
    private readonly IOutboxService _outboxService;
    private readonly IConnection _rabbitMqConnection;
    private readonly ILogger<PaymentPublisher> _logger;
    
    private const string QueueName = "payment_created";
    
    public PaymentPublisher(
        IOutboxService outboxService, 
        IConnection rabbitMqConnection,
        ILogger<PaymentPublisher> logger)
    {
        _outboxService = outboxService;
        _rabbitMqConnection = rabbitMqConnection;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var channel = await _rabbitMqConnection.CreateChannelAsync(cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(
            queue: QueueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null,
            cancellationToken: cancellationToken);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            // Получаем необработанные сообщения из Outbox
            var messages = await _outboxService.GetUnprocessedMessagesAsync();

            foreach (var message in messages)
            {
                // Отправляем сообщение в RabbitMQ
                var body = Encoding.UTF8.GetBytes(message.Payload);
                await channel.BasicPublishAsync(
                    exchange: string.Empty, 
                    routingKey: QueueName,
                    body: body,
                    cancellationToken: cancellationToken);

                // Помечаем сообщение как обработанное
                await _outboxService.MarkAsProcessedAsync(message.Id);
                _logger.LogInformation($"Сообщение отправлено в RabbitMQ: {message.Payload}");
            }

            await Task.Delay(5000, cancellationToken); // Проверяем каждые 5 секунд
        }
    }
}