using Common.Queue.Message;

namespace Common.Queue
{
    public interface IQueueProducer
    {
        public Task PublishMessageAsync(MessageContext context);
        public Task PublishMessageAsync(string exchangeName, string routingKey, MessageContext context);
    }
}
