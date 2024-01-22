namespace Domain.Interfaces
{
    public interface IProducer
    {
        Task Send(object message, string queue);
    }
}
