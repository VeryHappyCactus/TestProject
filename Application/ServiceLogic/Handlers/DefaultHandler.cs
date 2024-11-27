using AutoMapper;

using Common.Enums.Errors;
using Common.Queue;
using Common.Queue.Message;
using Common.Queue.Message.ClientOperation.Request;
using Common.Queue.Message.ClientOperation.Result;

using ServiceLogic.Exceptions;
using ServiceLogic.Handlers.CommonModels.Request;
using ServiceLogic.Handlers.Enums;
using ServiceLogic.Managers;

namespace ServiceLogic.Handlers
{
    public sealed class DefaultHandler : BaseHandler<DefaultRequest, object?>
    {
        private readonly EventWaitHandle _eventWaitHandle;
        private readonly string _key = Guid.NewGuid().ToString();
        private readonly object _lock = new object();
        private readonly TimeSpan _timeout;

        private object? _result;
        private DefaultRequest? _request;
        private Exception? _exception;
        private MessageEventHandlerStatuses? _handlerStatus;

        private Dictionary<string, ObjectTypeModel> _dicObjectTypeModels;

        public DefaultHandler(IQueueManager queueManager, IMapper mapper)
           : base(queueManager, mapper)
        {
            _eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _timeout = TimeSpan.FromSeconds(30);

            _dicObjectTypeModels = InitResultTypes();
        }

        private Dictionary<string, ObjectTypeModel> InitResultTypes()
        {
            return new Dictionary<string, ObjectTypeModel>()
            {
                [CommonClientOperationTypes.GetClientOperation.ToString()] = new ObjectTypeModel(typeof(ClientOperationRequestMessage), typeof(ClientOperationResultMessage)),
                [CommonClientOperationTypes.GetClientOperations.ToString()] = new ObjectTypeModel(typeof(ClientOperationsRequestMessage), typeof(MessageCollection)),
                [CommonClientOperationTypes.GetExchangeCourse.ToString()] = new ObjectTypeModel(typeof(ExchangeCourseRequestMessage), typeof(MessageCollection)),
            };
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
                    Body = (BaseMessage)_mapper.Map(request.Model, request.RequestType, _dicObjectTypeModels[_request!.Operation].MessageRequestType)
                });

                _eventWaitHandle.WaitOne(_timeout);

                lock (_lock)
                {
                    if (_result == null)
                    {
                        if (_handlerStatus.HasValue && _handlerStatus == MessageEventHandlerStatuses.Error && _exception != null)
                            throw _exception;
                        else if (!_handlerStatus.HasValue)
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
                        return;
                    
                    if (context.Body == null)
                    {
                        _handlerStatus = MessageEventHandlerStatuses.Error;
                        _exception = new HandlerException(nameof(CommonErrorTypes), (int)CommonErrorTypes.BadRequest, "Result message body is empty", HandlerErrorTypes.RequestError);
                        
                    }
                    else
                    {
                        _handlerStatus = MessageEventHandlerStatuses.Success;
                        _result = _mapper.Map(context.Body, _dicObjectTypeModels[_request!.Operation].MessageResultType, _request!.ResultType);
                    }
                }
            }
            finally
            {
                _eventWaitHandle.Set();
            }
        }

        private class ObjectTypeModel
        {
            public Type MessageRequestType { get; set; }
            public Type MessageResultType { get; set; }

            public ObjectTypeModel(Type messageRequestType, Type messageResultType)
            {
                MessageRequestType = messageRequestType;
                MessageResultType = messageResultType;
            }
        }
    }
}
