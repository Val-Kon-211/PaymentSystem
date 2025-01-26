using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orchestrator.Core.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Orchestrator.Service;

public class RabbitMqMessageHandler : BackgroundService 
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMqMessageHandler> _logger;

    public RabbitMqMessageHandler(
        IConnection connection, 
        IServiceProvider serviceProvider, 
        ILogger<RabbitMqMessageHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: "payment_created", 
            durable: true, 
            exclusive: false, 
            autoDelete: false,
            cancellationToken: cancellationToken);
        
        await channel.QueueDeclareAsync(
            queue: "transaction_created", 
            durable: true, 
            exclusive: false, 
            autoDelete: false,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            if (ea.RoutingKey == "payment_created")
            {
                var paymentEvent = JsonSerializer.Deserialize<PaymentCreatedEvent>(message);
                await ProcessPaymentEvent(paymentEvent);
            }
            else if (ea.RoutingKey == "transaction_created")
            {
                var transactionEvent = JsonSerializer.Deserialize<TransactionCreatedEvent>(message);
                await ProcessTransactionEvent(transactionEvent);
            }
        };

        await channel.BasicConsumeAsync(
            queue: "payment_created", 
            autoAck: true, 
            consumer: consumer,
            cancellationToken: cancellationToken);
        
        await channel.BasicConsumeAsync(
            queue: "transaction_created", 
            autoAck: true, 
            consumer: consumer,
            cancellationToken: cancellationToken);

        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    private async Task ProcessPaymentEvent(PaymentCreatedEvent paymentEvent)
    {
        _logger.LogInformation($"Processing payment: {paymentEvent.PaymentId}");

        // Publish orchestrator_payment_processed event
        await PublishMessage("orchestrator_payment_processed", paymentEvent);
    }

    private async Task ProcessTransactionEvent(TransactionCreatedEvent transactionEvent)
    {
        _logger.LogInformation($"Processing transaction: {transactionEvent.TransactionId}");

        // Publish orchestrator_transaction_processed event
        await PublishMessage("orchestrator_transaction_processed", transactionEvent);
    }

    private async Task PublishMessage(string queueName, object message)
    {
        var channel = await _connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
        await Task.CompletedTask;
    }
}