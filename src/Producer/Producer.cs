using Domain.Interfaces;
using RabbitMQ.Client;
using System.Text.Json;

namespace RabbitMQ.Producer;

public class Producer(ConnectionFactory factory) : IProducer
{
    private readonly ConnectionFactory _factory = factory;

    public Task Send(object message, string queue)
    {
        return Task.Run(() =>
        {
            _factory.HostName = "host.docker.internal";
            _factory.Port = AmqpTcpEndpoint.UseDefaultPort;

            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(
                queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var stringfiedMessage = JsonSerializer.Serialize(message);
            var bytesMessage = System.Text.Encoding.UTF8.GetBytes(stringfiedMessage);

            channel.BasicPublish(
                exchange: "",
                routingKey: queue,
                basicProperties: null,
                body: bytesMessage
                );
        });
    }
}
