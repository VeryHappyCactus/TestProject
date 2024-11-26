using System.Text.Json;

using Microsoft.Extensions.Logging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Common.Queue.Consumer;
using Common.Queue.Producer;
using Common.Queue.Settings;

namespace Common.Queue
{
    public class QueueConnectionFactory : IQueueConnectionFactory
    {
        private readonly ConnectionFactory _connectionFactoryConsumer;
        private readonly ConnectionFactory _connectionFactoryProducer;

        private readonly JsonSerializerOptions _jsonSerializerOption;
        private readonly QueueConnectionFactorySettings _settings;

        private readonly ILogger _logger;

        public QueueConnectionFactory(QueueConnectionFactorySettings settings, JsonSerializerOptions jsonSerializerOption, ILogger logger)
        {
            if (jsonSerializerOption == null)
                throw new ArgumentNullException(nameof(jsonSerializerOption));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _connectionFactoryConsumer = new ConnectionFactory 
            { 
                HostName = settings.ConsumerConncetionSettings!.HostName!, 
                Port = settings.ConsumerConncetionSettings!.Port, 
                UserName = settings.ConsumerConncetionSettings!.UserName!, 
                Password = settings.ConsumerConncetionSettings!.Password!
            };
            
            _connectionFactoryProducer = new ConnectionFactory 
            { 
                HostName = settings.ProducerConnectionSettings!.HostName!, 
                Port = settings.ProducerConnectionSettings!.Port, 
                UserName = settings.ProducerConnectionSettings!.UserName!, 
                Password = settings.ProducerConnectionSettings!.Password!
           };
            
            _connectionFactoryConsumer.AutomaticRecoveryEnabled = true;
            _connectionFactoryProducer.AutomaticRecoveryEnabled = true;

            _jsonSerializerOption = jsonSerializerOption;
            _settings = settings;
            _logger = logger;
        }

        public async Task<IQueueConsumer> CreateQueueConsumerAsync()
        {
            return await CreateQueueConsumerAsync
                (
                    queueName: _settings.ConsumerSettings!.QueueName!,
                    exchangeName: _settings.ConsumerSettings!.ExchangeName!,
                    routingKey: _settings.ConsumerSettings!.RoutingKey!,
                    isAutoAck: true
                );
        }

        public async Task<IQueueProducer> CreateQueueProducerAsync()
        {
            return await CreateQueueProducerAsync
                (
                    queueName: _settings.ProducerSettings!.QueueName!,
                    exchangeName: _settings.ProducerSettings!.ExchangeName!,
                    routingKey: _settings.ProducerSettings!.RoutingKey!
                );
        }
        
        public async Task<IQueueConsumer> CreateQueueConsumerAsync(string queueName, string exchangeName, string routingKey, bool isAutoAck = true)
        {
            IConnection connection = await _connectionFactoryConsumer.CreateConnectionAsync();
            IChannel channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false);
            await channel.QueueDeclareAsync(queueName, true, false, false);

            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

            await channel.QueueBindAsync(queue: queueName,
                                exchange: exchangeName,
                                routingKey: routingKey);

            await channel.BasicConsumeAsync(queue: queueName,
                                 autoAck: isAutoAck,
                                 consumer: consumer);

            return new QueueConsumer(consumer, _jsonSerializerOption, _logger);
        }

        public async Task<IQueueProducer> CreateQueueProducerAsync(string queueName, string exchangeName, string routingKey)
        {
            IConnection connection = await _connectionFactoryProducer.CreateConnectionAsync();
            IChannel channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false);
            await channel.QueueDeclareAsync(queueName, true, false, false);

            await channel.QueueBindAsync(queue: queueName,
                              exchange: exchangeName,
                              routingKey: routingKey);

            return new QueueProducer(channel, exchangeName, routingKey, _jsonSerializerOption);
        }
    }
}
