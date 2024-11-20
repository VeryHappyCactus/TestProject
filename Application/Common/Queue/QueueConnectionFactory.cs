using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Common.Queue.Consumer;
using Common.Queue.Producer;
using Common.Settings;
using Common.Secret;
using Microsoft.Extensions.Logging;

namespace Common.Queue
{
    public class QueueConnectionFactory : IQueueConnectionFactory
    {
        private readonly ConnectionFactory _connectionFactoryConsumer;
        private readonly ConnectionFactory _connectionFactoryProducer;

        private readonly IAppCommonSettings _appCommonSettings;
        private readonly ISecretManager _secretManager;
        private readonly ILogger _logger;
     
        public QueueConnectionFactory(ISecretManager secretManager, IAppCommonSettings appCommonSettings, ILogger logger)
        {
            if (appCommonSettings == null)
                throw new ArgumentNullException(nameof(appCommonSettings));

            if (secretManager == null)
                throw new ArgumentNullException(nameof(secretManager));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _connectionFactoryConsumer = new ConnectionFactory 
            { 
                HostName = secretManager.SecretSettings.ConnectionFactorySettings!.ConsumerConncetionSettings!.HostName!, 
                Port = secretManager.SecretSettings.ConnectionFactorySettings!.ConsumerConncetionSettings!.Port, 
                UserName = secretManager.SecretSettings.ConnectionFactorySettings!.ConsumerConncetionSettings!.UserName!, 
                Password = secretManager.SecretSettings.ConnectionFactorySettings!.ConsumerConncetionSettings!.Password!
            };
            
            _connectionFactoryProducer = new ConnectionFactory 
            { 
                HostName = secretManager.SecretSettings.ConnectionFactorySettings!.ProducerConnectionSettings!.HostName!, 
                Port = secretManager.SecretSettings.ConnectionFactorySettings!.ProducerConnectionSettings!.Port, 
                UserName = secretManager.SecretSettings.ConnectionFactorySettings!.ProducerConnectionSettings!.UserName!, 
                Password = secretManager.SecretSettings.ConnectionFactorySettings!.ProducerConnectionSettings!.Password!
           };
            
            _connectionFactoryConsumer.AutomaticRecoveryEnabled = true;
            _connectionFactoryProducer.AutomaticRecoveryEnabled = true;

            _appCommonSettings = appCommonSettings;
            _secretManager = secretManager;
            _logger = logger;
        }

        public async Task<IQueueConsumer> CreateQueueConsumerAsync()
        {
            return await CreateQueueConsumerAsync
                (
                    queueName: _secretManager.SecretSettings!.ConnectionFactorySettings!.ConsumerSettings!.QueueName!,
                    exchangeName: _secretManager.SecretSettings!.ConnectionFactorySettings!.ConsumerSettings!.ExchangeName!,
                    routingKey: _secretManager.SecretSettings!.ConnectionFactorySettings!.ConsumerSettings!.RoutingKey!,
                    isAutoAck: true
                );
        }

        public async Task<IQueueProducer> CreateQueueProducerAsync()
        {
            return await CreateQueueProducerAsync
                (
                    queueName: _secretManager.SecretSettings!.ConnectionFactorySettings!.ProducerSettings!.QueueName!,
                    exchangeName: _secretManager.SecretSettings!.ConnectionFactorySettings!.ProducerSettings!.ExchangeName!,
                    routingKey: _secretManager.SecretSettings!.ConnectionFactorySettings!.ProducerSettings!.RoutingKey!
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

            return new QueueConsumer(consumer, _appCommonSettings, _logger);
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

            return new QueueProducer(channel, exchangeName, routingKey, _appCommonSettings);
        }
    }
}
