using Common.Queue.Consumer;
using Common.Queue.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorHandlers.Managers
{
    public interface IQueueManager
    {
        //public Task Initialize();

        //public IQueueConsumer QueueConsumer { get; }
        //public IQueueProducer QueueProducer { get; }

        public Task<IQueueConsumer> GetOrCreateQueueConsumer();
        public Task<IQueueProducer> GetOrCreateQueueProducer();
    }
}
