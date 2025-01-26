using System.Text.Json;
using Payment.Core.DataAccess.Repositories;
using Payment.Core.Models;
using Payment.Service.Events;
using Payment.Service.Interfaces;

namespace Payment.Service.Services;

public class PaymentService: IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMessageProducerService _messageProducerService;
    
    public PaymentService(
        IPaymentRepository paymentRepository,
        IMessageProducerService messageProducerService)
    {
        _paymentRepository = paymentRepository;
        _messageProducerService = messageProducerService;
    }
    
    public async Task ProcessPaymentAsync(PaymentEntity payment)
    {
        // Заглушка для имитации работы с платежной системой
        payment.Status = new Random().Next(0, 2) == 0 ? PaymentStatus.Success : PaymentStatus.Failed;
        
        await _paymentRepository.AddAsync(payment);
        
        var payload = JsonSerializer.Serialize(new PaymentCreatedEvent
        {
            PaymentId = payment.Id,
            Status = payment.Status.ToString()
        });

        await _messageProducerService.CreateMessageAsync("", payload);
    }
}