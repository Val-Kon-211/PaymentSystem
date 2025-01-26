namespace Orchestrator.Core.Events;

public class PaymentCreatedEvent
{
    public int PaymentId { get; set; }
    public string Status { get; set; }
}