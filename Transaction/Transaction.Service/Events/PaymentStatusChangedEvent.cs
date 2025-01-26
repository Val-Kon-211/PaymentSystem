namespace Transaction.Service.Events;

public class PaymentStatusChangedEvent
{
    public int PaymentId { get; set; }
    public string Status { get; set; }
}