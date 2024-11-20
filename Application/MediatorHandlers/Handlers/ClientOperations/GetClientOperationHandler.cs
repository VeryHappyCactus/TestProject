using AutoMapper;

using Common.Enums.Errors;
using Common.Queue.Consumer;
using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Message;
using Common.Queue.Producer;

using MediatorHandlers.Managers;
using MediatorHandlers.Handlers.ClientOperations.Request;
using MediatorHandlers.Handlers.ClientOperations.Result;

namespace MediatorHandlers.Handlers.ClientOperations
{
    public class GetClientOperationHandler : BaseHandler<GetClientOperationRequest, HandlerResultContext<GetClientOperationResult>>
    {
        private readonly string _key = Guid.NewGuid().ToString();
        private readonly object _lock = new object();

        private HandlerResultContext<GetClientOperationResult>? _result;

        public GetClientOperationHandler(IQueueManager queueManager, IMapper mapper)
            : base(queueManager, mapper) { }

        public override async Task<HandlerResultContext<GetClientOperationResult>> Handle(GetClientOperationRequest request, CancellationToken cancellationToken)
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
                    Body = _mapper.Map<ClientOperationRequestMessage>(request)
                });

                _eventWaitHandle.WaitOne(_timeout);

                lock (_lock)
                {
                    if (_result == null)
                    {
                        return new HandlerResultContext<GetClientOperationResult>()
                        {
                            Error = new HandlerErrorContext()
                            {
                                Message = "Message did not delivery",
                                ErrorCode = (int)CommonErrorTypes.Internal
                            }
                        };
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

        public override async Task OnAsyncMessageEventHadler(MessageContext context)
        {

            try
            {
                lock (_lock)
                {
                    if (_result != null)
                        return;

                    if (context.Body is ErrorMessage errorMessage)
                    {
                        _result = new HandlerResultContext<GetClientOperationResult>()
                        {
                            Error = new HandlerErrorContext()
                            {
                                ErrorCode = errorMessage.ErrorCode,
                                Message = errorMessage.ErrorCode switch
                                {
                                    (int)CommonErrorTypes.Internal => "Internal backend error",
                                    _ => null
                                }
                            }
                        };
                    }
                    else
                    {
                        _result = new HandlerResultContext<GetClientOperationResult>()
                        {
                            Value = _mapper.Map<GetClientOperationResult>(context.Body)
                        };
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
