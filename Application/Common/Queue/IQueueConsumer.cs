using Common.Queue.Consumer.Delegates;

namespace Common.Queue
{
    public interface IQueueConsumer
    {
        public void StartConsume();
        public bool TryAddConsumer(string key, AsyncMessageEventHadler eventHandler);
        public void AddConsumer(AsyncMessageEventHadler eventHandler);
        public bool TryRemoveConsumer(string key);

    }
}
