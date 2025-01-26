namespace Transaction.Service.Interfaces;

public interface IMessageProducerService
{
    Task CreateMessageAsync(string messageType, string messagePayload);
}