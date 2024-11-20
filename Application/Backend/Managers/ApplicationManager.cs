using Microsoft.Extensions.Logging;

using Common.Queue;
using Common.Queue.Consumer;
using Common.Queue.Message;
using Common.Queue.Producer;
using Common.Enums.Errors;

using Backend.JobFactories;
using Backend.Jobs;

namespace Backend.Queue
{
    internal class ApplicationManager : IApplicationManager
    {
        private IQueueConsumer? _queueConsumer;
        private IQueueProducer? _queueProducer;

        private readonly IQueueConnectionFactory _queueConnectionFactory;
        private readonly IJobFactory _jobFactory;
        private readonly ILogger _logger;

        public ApplicationManager(IQueueConnectionFactory queueConnectionFactory, IJobFactory jobFactory, ILogger logger) 
        {
            if (queueConnectionFactory == null)
                throw new ArgumentNullException(nameof(queueConnectionFactory));
            
            if (jobFactory == null)
                throw new ArgumentNullException(nameof(jobFactory));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            
            _queueConnectionFactory = queueConnectionFactory;
            _jobFactory = jobFactory;
            _logger = logger;
        }

        public async Task Run()
        {
            _queueConsumer = await _queueConnectionFactory.CreateQueueConsumerAsync();
            _queueProducer = await ((QueueConnectionFactory)_queueConnectionFactory).CreateQueueProducerAsync();

            _queueConsumer!.AddConsumer(OnAsyncMessageEventHadler);
            _queueConsumer!.StartConsume();
        }

        public async Task OnAsyncMessageEventHadler(MessageContext context)
        {
            MessageContext? messageContext = null;

            try
            {
                IJob job = _jobFactory.CreateJob(context.Body!);

                messageContext = new MessageContext()
                {
                    PublisherId = context.PublisherId,
                    SessionId = context.SessionId,
                    RequestId = context.RequestId,
                    Body = await job.ExecuteAsync(context.Body!)
                };

                await _queueProducer!.PublishMessageAsync(messageContext);
            }
            catch (Exception ex)
            {
                await _queueProducer!.PublishMessageAsync(new MessageContext()
                {
                    PublisherId = context.PublisherId,
                    SessionId = context.SessionId,
                    RequestId = context.RequestId,
                    Body = new ErrorMessage()
                    {
                        ErrorCode = (int)CommonErrorTypes.Internal
                    }
                });

                throw;
            }
        }
    }
}
