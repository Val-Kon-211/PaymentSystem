namespace Transaction.Service.Events;

public class TransactionCreatedEvent
{
    public int TransactionId { get; set; }
    public int PaymentId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}