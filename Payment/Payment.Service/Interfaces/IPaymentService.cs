using Payment.Core.Models;

namespace Payment.Service.Interfaces;

public interface IPaymentService
{
    Task ProcessPaymentAsync(PaymentEntity payment);
}