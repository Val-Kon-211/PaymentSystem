using System.Text.Json;
using Transaction.Core.DataAccess.Repositories;
using Transaction.Core.Models;
using Transaction.Service.Events;
using Transaction.Service.Interfaces;

namespace Transaction.Service.Services;

public class TransactionService: ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageProducerService _messageProducerService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IMessageProducerService messageProducerService)
    {
        _transactionRepository = transactionRepository;
        _messageProducerService = messageProducerService;
    }

    // Обработка события о изменении статуса платежа
    public async Task HandlePaymentStatusChangedEvent(PaymentStatusChangedEvent @event)
    {
        // Создание транзакции на основе события
        var transaction = new TransactionEntity
        {
            PaymentId = @event.PaymentId,
            Status = ConvertToTransactionStatus(@event.Status),
            CreatedAt = DateTime.UtcNow
        };

        // Сохранение транзакции в базу данных
        await _transactionRepository.AddAsync(transaction);

        var payload = JsonSerializer.Serialize(new TransactionCreatedEvent
        {
            TransactionId = transaction.Id,
            PaymentId = transaction.PaymentId,
            Status = transaction.Status.ToString(),
            CreatedAt = transaction.CreatedAt
        });

        await _messageProducerService.CreateMessageAsync("", payload);
    }

    private TransactionStatus ConvertToTransactionStatus(string status)
    {
        if (status == TransactionStatus.Pending.ToString()) return TransactionStatus.Pending;
        
        if (status == TransactionStatus.Failed.ToString()) return TransactionStatus.Failed;
        
        return TransactionStatus.Success;
    }
}