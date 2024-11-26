using Common.Queue;

namespace ServiceLogic.Managers
{
    public class QueueManager : IQueueManager
    {
        private IQueueConsumer? _queueConsumer = null;
        private IQueueProducer? _queueProducer = null;

        private readonly IQueueConnectionFactory _queueConnectionFactory;

        private SemaphoreSlim _semaphoreQueueConsumer = new SemaphoreSlim(1, 1);
        private SemaphoreSlim _semaphoreQueueProducer = new SemaphoreSlim(1, 1);

        public QueueManager(IQueueConnectionFactory queueConnectionFactory)
        {
            if (queueConnectionFactory == null)
                throw new ArgumentNullException(nameof(queueConnectionFactory));

            _queueConnectionFactory = queueConnectionFactory;
        }

        public async Task<IQueueConsumer> GetOrCreateQueueConsumer()
        {
            try
            {
                if (_queueConsumer == null)
                {
                    _semaphoreQueueConsumer.Wait();
                
                    if (_queueConsumer == null)
                    {
                        _queueConsumer = await _queueConnectionFactory.CreateQueueConsumerAsync();
                        _queueConsumer.StartConsume();
                    }
                }
            }
            finally
            {
                if (_semaphoreQueueConsumer.CurrentCount == 1)
                    _semaphoreQueueConsumer.Release();
            }

            return _queueConsumer;
        }

        public async Task<IQueueProducer> GetOrCreateQueueProducer()
        {
            try
            {
                if (_queueProducer == null)
                {
                    _semaphoreQueueProducer.Wait();

                    if (_queueProducer == null)
                    {
                        _queueProducer = await ((QueueConnectionFactory)_queueConnectionFactory).CreateQueueProducerAsync();
                    }
                }
            }
            finally
            {
                if (_semaphoreQueueProducer.CurrentCount == 1)
                _semaphoreQueueProducer.Release();
            }

            return _queueProducer;
        }

    }
}
