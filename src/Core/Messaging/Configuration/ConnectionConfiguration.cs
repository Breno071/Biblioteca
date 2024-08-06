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

            _channel.BasicPublish(exchange: message.GetType().ToString(),
                                     routingKey: Contract.QueueName,
                                     basicProperties: null,
                                     body: bytes);
        }

        public void PublishLargeMessage(object message, string identifier)
        {
            var json = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(json);

            int remainingFileSize = Convert.ToInt32(bytes.Length);

            bool finished = false;
            byte[] buffer;
            int index = 0;
            int number = 0;

            while (true)
            {
                if (remainingFileSize <= 0)
                {
                    break;
                }

                int read;

                if (remainingFileSize > Contract.ChunkSize)
                {
                    buffer = new byte[Contract.ChunkSize];
                    Array.Copy(bytes, index, buffer, 0, Contract.ChunkSize);
                    index += Contract.ChunkSize;
                    read = Contract.ChunkSize;
                }
                else
                {
                    buffer = new byte[remainingFileSize];
                    Array.Copy(bytes, index, buffer, 0, remainingFileSize);
                    read = remainingFileSize;
                    finished = true;
                }

                IBasicProperties basicProperties = _channel.CreateBasicProperties();
                basicProperties.Headers = new Dictionary<string, object>
                {
                    { Contract.IsChunk, true },
                    { Contract.Identifier, identifier },
                    { Contract.Number, number },
                    { Contract.Finished, finished }
                };

                _channel.BasicPublish(message.GetType().ToString(), routingKey: Contract.QueueName, basicProperties, buffer);
                Console.WriteLine($"Chunk published with size {buffer.Length}, IsLast: {finished}.");
                remainingFileSize -= read;
                number++;
            }

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
