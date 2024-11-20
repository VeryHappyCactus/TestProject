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
        private readonly IAppCommonSettings _appCommonSettings;
        private readonly string _exchangeName;
        private readonly string _routingKey;

        public QueueProducer(IChannel channel, string exchangeName, string routingKey, IAppCommonSettings appCommonSettings) 
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            if (appCommonSettings == null)
                throw new ArgumentNullException(nameof(appCommonSettings));

            if (exchangeName == null)
                throw new ArgumentNullException(nameof(exchangeName));

            if (routingKey == null)
                throw new ArgumentNullException(nameof(routingKey));

            _channel = channel;
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _appCommonSettings = appCommonSettings;
        }

        public async Task PublishMessageAsync(MessageContext context)
        {
            await PublishMessageAsync(_exchangeName, _routingKey, context);
        }

        public async Task PublishMessageAsync(string exchangeName, string routingKey, MessageContext context)
        {
            string json = JsonSerializer.Serialize(context, _appCommonSettings.JsonSettings.JsonSerializerOption);
            byte[] data = Encoding.UTF8.GetBytes(json);
            await _channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: data);
        }
    }
}
