using Transaction.Core.Models;
using Transaction.Service.Events;

namespace Transaction.Service.Interfaces;

public interface ITransactionService
{
    Task HandlePaymentStatusChangedEvent(PaymentStatusChangedEvent @event);
}