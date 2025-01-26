using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Transaction.Service.Events;
using Transaction.Service.Interfaces;

namespace Transaction.Service.Services.RabbitMq;

public class TransactionConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionConsumer> _logger;
    
    private const string QueueName = "orchestrator_payment_processed";

    public TransactionConsumer(
        IConnection connection, 
        ITransactionService transactionService,
        ILogger<TransactionConsumer> logger)
    {
        _connection = connection;
        _transactionService = transactionService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(
            queue: QueueName, 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("TransactionConsumer инициализирован.");
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<PaymentStatusChangedEvent>(message);

                _logger.LogInformation($"Получено сообщение: {message}");

                await _transactionService.HandlePaymentStatusChangedEvent(@event);

                _logger.LogInformation("Сообщение успешно обработано.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке сообщения.");
            }
        };

        await channel.BasicConsumeAsync(queue: QueueName, autoAck: true, consumer: consumer);
        
        // Ожидаем завершения работы
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000, cancellationToken);
        }

        _logger.LogInformation("TransactionConsumer остановлен.");
    }
}