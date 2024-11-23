using AutoMapper;
using MediatR;

using Common.Queue.Consumer.Delegates;
using Common.Queue.Message;

using Common.Enums.Errors;
using Common.Queue.Consumer;
using Common.Queue.Producer;

using MediatorHandlers.Managers;
using MediatorHandlers.Exceptions;

namespace MediatorHandlers.Handlers
{
    public abstract class BaseHandler<THandlerRequest, THandlerResponse, TRequestMessage> : IRequestHandler<THandlerRequest, THandlerResponse>
        where THandlerRequest : IRequest<THandlerResponse>
        where TRequestMessage : BaseMessage, new()
    {
        protected readonly IQueueManager _queueManager;
        protected readonly IMapper _mapper;
        protected readonly EventWaitHandle _eventWaitHandle;
        protected readonly AsyncMessageEventHadler _eventHandler;

        protected readonly string _key = Guid.NewGuid().ToString();
        protected readonly object _lock = new object();
        protected readonly TimeSpan _timeout;

        protected THandlerResponse _result;

        public BaseHandler(IQueueManager queueManager, IMapper mapper)
        {
            if (queueManager == null)
                throw new ArgumentNullException(nameof(queueManager));
            
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            _queueManager = queueManager;
            _mapper = mapper;
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _timeout = TimeSpan.FromSeconds(30_000);
            _eventHandler = OnAsyncMessageEventHadler;
        
        }
        public virtual async Task<THandlerResponse> Handle(THandlerRequest request, CancellationToken cancellationToken)
        {
            IQueueConsumer? consumer = null;
            IQueueProducer? producer = null;

            try
            {
                consumer = await _queueManager.GetOrCreateQueueConsumer();
                producer = await _queueManager.GetOrCreateQueueProducer();

                consumer.TryAddConsumer(_key, _eventHandler);

                await producer.PublishMessageAsync(new MessageContext()
                {
                    PublisherId = _key,
                    SessionId = _key,
                    RequestId = _key,
                    Body = _mapper.Map<TRequestMessage>(request)
                });

                _eventWaitHandle.WaitOne(_timeout);

                lock (_lock)
                {
                    if (_result == null)
                    {
                        throw new HandlerException(nameof(CommonErrorTypes), (int)CommonErrorTypes.Internal, "Timeout exception, result did not get in time", HandlerErrorTypes.Internal);
                    }
                }

            }
            finally
            {
                if (consumer != null)
                    consumer.TryRemoveConsumer(_key);
            }

            return _result;
        }

        public virtual async Task OnAsyncMessageEventHadler(MessageContext context)
        {
            try
            {
                lock (_lock)
                {
                    if (_result != null)
                    {
                        throw new HandlerException(nameof(CommonErrorTypes), (int)CommonErrorTypes.Internal, "Result message body is empty", HandlerErrorTypes.Internal);
                    }

                    else
                    {
                        _result = _mapper.Map<THandlerResponse>(context.Body);
                    }
                }
            }
            finally
            {
                _eventWaitHandle.Set();
            }
        }
    }
}
