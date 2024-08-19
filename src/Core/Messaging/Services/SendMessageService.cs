using Core.Messaging.Configuration;
using Microsoft.Extensions.Configuration;

namespace Core.Messaging.Services
{
    public interface ISendMessage
    {
        void Send(object message);
    }

    public class SendMessageService : ISendMessage
    {
        private readonly ConnectionConfiguration broker;

        public SendMessageService(IConfiguration configuration)
        {
            broker = new ConnectionConfiguration(configuration);
        }

        public void Send(object message)
        {
            broker.PublishMessage(message);
        }
    }
}
