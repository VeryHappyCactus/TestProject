namespace Common.Queue
{
    public interface IQueueConnectionFactory
    {
        public Task<IQueueConsumer> CreateQueueConsumerAsync();
        public Task<IQueueProducer> CreateQueueProducerAsync();
        public Task<IQueueConsumer> CreateQueueConsumerAsync(string queueName, string exchangeName, string routingKey, bool isAutoAck = true);
        public Task<IQueueProducer> CreateQueueProducerAsync(string queueName, string exchangeName, string routingKey);
    }
}
