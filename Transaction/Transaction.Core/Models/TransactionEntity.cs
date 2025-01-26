namespace Transaction.Core.Models;

public class TransactionEntity
{
    public int Id { get; set; }
    
    /// <summary>
    /// Идентификатор платежа
    /// </summary>
    public int PaymentId { get; set; }
    
    /// <summary>
    /// Время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Статус транзакции
    /// </summary>
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
}