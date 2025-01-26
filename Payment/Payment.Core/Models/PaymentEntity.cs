namespace Payment.Core.Models;

public class PaymentEntity
{
    public int Id { get; set; }
    
    /// <summary>
    /// Сумма платежа
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Валюда
    /// </summary>
    public string Currency { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Статус платежа
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
}