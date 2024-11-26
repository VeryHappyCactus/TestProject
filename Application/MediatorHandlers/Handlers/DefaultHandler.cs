using AutoMapper;

using Common.Enums.Errors;
using Common.Queue.Consumer;
using Common.Queue.Message;
using Common.Queue.Producer;

using MediatorHandlers.Exceptions;
using MediatorHandlers.Handlers.ClientOperations.Result;
using MediatorHandlers.Handlers.CommonModels.Request;
using MediatorHandlers.Handlers.Enums;
using MediatorHandlers.Managers;

namespace MediatorHandlers.Handlers
{
    public sealed class DefaultHandler : BaseHandler<DefaultRequest, object?>
    {
        private readonly EventWaitHandle _eventWaitHandle;
        private readonly string _key = Guid.NewGuid().ToString();
        private readonly object _lock = new object();
        private readonly TimeSpan _timeout;

        private object? _result;
        private DefaultRequest? _request;

        private Dictionary<string, Type> _dictionaryResultTypes;

        public DefaultHandler(IQueueManager queueManager, IMapper mapper)
           : base(queueManager, mapper)
        {
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _timeout = TimeSpan.FromSeconds(30_000);

            _dictionaryResultTypes = InitResultTypes();
        }

        public override async Task<object?> Handle(DefaultRequest request, CancellationToken cancellationToken)
        {
            IQueueConsumer? consumer = null;
            IQueueProducer? producer = null;

            try
            {
                _request = request;

                consumer = await _queueManager.GetOrCreateQueueConsumer();
                producer = await _queueManager.GetOrCreateQueueProducer();

                consumer.TryAddConsumer(_key, _eventHandler);

                await producer.PublishMessageAsync(new MessageContext()
                {
                    PublisherId = _key,
                    SessionId = _key,
                    RequestId = _key,
                    Body = (BaseMessage)_mapper.Map(request.Model, request.Model.GetType(), request.RequestType)
                });

                _eventWaitHandle.WaitOne(_timeout);

                lock (_lock)
                {
                    if (_result == null)
                    {
                        throw new HandlerException(nameof(CommonErrorTypes), (int)CommonErrorTypes.BadRequest, "Timeout exception, result did not get in time", HandlerErrorTypes.RequestError);
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
                    {
                        throw new HandlerException(nameof(CommonErrorTypes), (int)CommonErrorTypes.Internal, "Result message body is empty", HandlerErrorTypes.Internal);
                    }

                    else
                    {
                        _result = _mapper.Map(context.Body, _dictionaryResultTypes[_request!.Operation], _request!.ResultType);
                    }
                }
            }
            finally
            {
                _eventWaitHandle.Set();
            }
        }

        private Dictionary<string, Type> InitResultTypes()
        {
            return new Dictionary<string, Type>()
            {
                [CommonClientOperationTypes.GetClientOperation.ToString()] = typeof(GetClientOperationResult),
                [CommonClientOperationTypes.GetClientOperations.ToString()] = typeof(GetClientOperationResult),
                [CommonClientOperationTypes.GetExchangeCourse.ToString()] = typeof(GetExchangeCourseResult)
            };
        }
    }
}
