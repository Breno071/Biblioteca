using Core.Messaging.QueueCommon;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Core.Messaging.Configuration
{
    public class ConnectionConfiguration : IDisposable
    {
        private IConnection _connection;
        private IModel _channel;
        private Contract Contract = new Contract();

        public IModel Channel { get { return _channel; } }

        public ConnectionConfiguration(IConfiguration configuration)
        {
            configuration.GetSection(nameof(Contract)).Bind(Contract);
            InitQueue();
        }

        private void InitQueue()
        {
            var factory = new ConnectionFactory()
            {
                HostName = Contract.HostName,
                Port = AmqpTcpEndpoint.UseDefaultPort,
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: Contract.QueueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
            arguments: null);

            Console.WriteLine($"Queue {Contract.QueueName} declared.");
        }

        public void PublishMessage(object message)
        {
            var json = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(json);
            var exchange = message.GetType().ToString();

            _channel.BasicPublish(exchange: exchange,
                                     routingKey: Contract.QueueName,
                                     basicProperties: null,
                                     body: bytes);
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            _channel.Close();
            _channel.Dispose();
        }
    }
}
