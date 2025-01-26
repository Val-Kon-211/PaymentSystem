using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Report.Service.Events;
using Report.Service.Interfaces;

namespace Report.Service.Services.RabbitMq;

public class ReportConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IReportService _reportService;
    private readonly ILogger<ReportConsumer> _logger;
    
    private const string QueueName = "orchestrator_transaction_processed";

    public ReportConsumer(
        IConnection connection, 
        IReportService reportService,
        ILogger<ReportConsumer> logger)
    {
        _connection = connection;
        _reportService = reportService;
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
        
        _logger.LogInformation("ReportConsumer инициализирован.");
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<TransactionCreatedEvent>(message);

                _logger.LogInformation($"Получено сообщение: {message}");

                await _reportService.HandleTransactionCreatedEvent(@event);

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

        _logger.LogInformation("ReportConsumer остановлен.");
    }
}