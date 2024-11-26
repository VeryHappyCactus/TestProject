using Common.Queue;

namespace ServiceLogic.Managers
{
    public interface IQueueManager
    {
        public Task<IQueueConsumer> GetOrCreateQueueConsumer();
        public Task<IQueueProducer> GetOrCreateQueueProducer();
    }
}
