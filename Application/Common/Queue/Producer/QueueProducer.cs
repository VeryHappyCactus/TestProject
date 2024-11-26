using System.Text;
using System.Text.Json;

using RabbitMQ.Client;

using Common.Queue.Message;
using Common.Settings;

namespace Common.Queue.Producer
{
    public class QueueProducer : IQueueProducer
    {
        private readonly IChannel _channel;
        
        private readonly string _exchangeName;
        private readonly string _routingKey;

        private readonly JsonSerializerOptions _jsonSerializerOption;

        public QueueProducer(IChannel channel, string exchangeName, string routingKey, JsonSerializerOptions jsonSerializerOption) 
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            if (jsonSerializerOption == null)
                throw new ArgumentNullException(nameof(jsonSerializerOption));

            if (exchangeName == null)
                throw new ArgumentNullException(nameof(exchangeName));

            if (routingKey == null)
                throw new ArgumentNullException(nameof(routingKey));

            _channel = channel;
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _jsonSerializerOption = jsonSerializerOption;
        }

        public async Task PublishMessageAsync(MessageContext context)
        {
            await PublishMessageAsync(_exchangeName, _routingKey, context);
        }

        public async Task PublishMessageAsync(string exchangeName, string routingKey, MessageContext context)
        {
            string json = JsonSerializer.Serialize(context, _jsonSerializerOption);
            byte[] data = Encoding.UTF8.GetBytes(json);
            await _channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: data);
        }
    }
}
